
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Model.People;
using TaskManager.DomainLayer.Model.Tasks;
using TaskManager.DomainLayer.Operations;
using TaskManager.DomainLayer.Service.Database.Operations;

namespace TaskManager.DomainLayer.Repositories
{
    internal static class DevTaskRepository
    {
        private const string TableName = "DevTasks";

        // initialize methods
        internal static void Initialize()
        {
            using (var connection = DatabaseConnection.CreateConnection("inicializar tabela DevTasks"))
            {
                CreateDevTasksTable(connection);
                Console.WriteLine();
                DatabaseConnection.CloseConnection(connection, "inicializar tabela DevTasks");
            }

            InitializeDefaultTasks();
        }
        private static void InitializeDefaultTasks()
        {
            SQLiteConnection defaultConnection = null;

            try
            {
                Message.InitializeDefaultDevTasks();

                defaultConnection = DatabaseConnection.CreateConnection("inserir tarefas padrão em DevTasks");
                var tasks = GetTaskList();

                if (tasks.Count == 0)
                {
                    InsertDefaultTasks(defaultConnection);
                }
            }
            finally
            {
                Console.WriteLine();
                DatabaseConnection.CloseConnection(defaultConnection, "inserir tarefas padrão em DevTasks");
            }
        }
        private static void InsertDefaultTasks(SQLiteConnection connection)
        {
            InsertTaskIfNotExists(connection, new DevTask(
                techLeaderLogin: "kaio",
                title: "Implementar Funcionalidade X",
                deadline: DateTime.Now.AddDays(7),
                developerLogin: "kaio"
            ));

            InsertTaskIfNotExists(connection, new DevTask(
                techLeaderLogin: "kaio",
                title: "Corrigir bug Y",
                deadline: DateTime.Now.AddDays(3),
                developerLogin: "carolina"
            ));

            InsertTaskIfNotExists(connection, new DevTask(
                techLeaderLogin: "kaio",
                title: "Implementar Funcionalidade Z",
                deadline: DateTime.Now.AddDays(10)
            ));
        }
        internal static void InitializeNewDevTask(DevTask newTask)
        {
            SQLiteConnection defaultConnection = null;

            try
            {
                Message.InitializeDefaultDevTasks();

                defaultConnection = DatabaseConnection.CreateConnection("inserir nova tarefa em DevTasks");
                var tasks = GetTaskList();

                InsertTaskIfNotExists(defaultConnection, newTask);
            }
            finally
            {
                Console.WriteLine();
                DatabaseConnection.CloseConnection(defaultConnection, "inserir nova tarefa em DevTasks");
            }
        }

        // create methods
        private static void CreateDevTasksTable(SQLiteConnection connection)
        {
            const string createDevTasksQuery = $@"

                    CREATE TABLE {TableName} (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Description TEXT,
                    TechLeaderLogin TEXT NOT NULL,
                    DeveloperLogin TEXT,
                    Status TEXT NOT NULL,
                    RequiresApprovalToComplete TEXT NOT NULL,
                    Deadline DATETIME,
                    CompletionDateTime DATETIME,
                    FOREIGN KEY (TechLeaderLogin) REFERENCES Users(Login),
                    FOREIGN KEY (DeveloperLogin) REFERENCES Users(Login)
                    )";

            Tables.Create(connection, TableName, createDevTasksQuery);
        }

        // insert methods
        public static void AddDevTaskToDatabase(DevTask task)
        {
            DatabaseConnection.ExecuteWithinTransaction((transactionConnection) =>
            {
                try
                {
                    InsertTaskIfNotExists(transactionConnection, task);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro executando transação: {ex.Message}");
                    throw;
                }
            });
        }
        internal static void InsertTaskIfNotExists(SQLiteConnection connection, DevTask task)
        {
            try
            {
                if (!DoesTaskExist(connection, task))
                {
                    InsertTask(task);
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Erro ao inserir as tarefas DevTasks na tabela: {ex.Message}");
            }
        }
        private static void InsertTask(DevTask task)
        {
            try
            {
                string insertTaskQuery = $@"
                    INSERT INTO {TableName} (Title, Description, TechLeaderLogin, DeveloperLogin, Status, RequiresApprovalToComplete, Deadline, CompletionDateTime)
                    VALUES (@Title, @Description, @TechLeaderLogin, @DeveloperLogin, @Status, @RequiresApprovalToComplete, @Deadline, @CompletionDateTime);";

                var parameters = new Dictionary<string, object>
                {
                    { "@Title", task.Title },
                    { "@Description", task.Description },
                    { "@TechLeaderLogin", task.TechLeaderLogin },
                    { "@DeveloperLogin", task.DeveloperLogin },
                    { "@Status", task.Status },
                    { "@RequiresApprovalToComplete", task.RequiresApprovalToComplete },
                    { "@Deadline", task.Deadline },
                    { "@CompletionDateTime", task.CompletionDateTime },
                };

                DatabaseConnection.ExecuteNonQuery(insertTaskQuery, parameters);
                Console.WriteLine($"Task '{task.Title}' inserida com sucesso na tabela.");
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Erro ao inserir DevTask na tabela: {ex.Message}");
            }
        }
        
        // validation methods
        private static bool DoesTaskExist(SQLiteConnection connection, DevTask task)
        {

            string query = $"SELECT COUNT(*) FROM {TableName} WHERE Title = @Title AND Description = @Description;";

            var parameters = new Dictionary<string, object>
            {
                { "@Title", task.Title },
                { "@Description", task.Description }
            };

            int count = Convert.ToInt32(DatabaseConnection.ExecuteScalar(connection, query, parameters));

            return count > 0;
        }

        // read and display methods
        internal static List<DevTask> GetTaskList()
        {
            try
            {
                const string query = "SELECT * FROM DevTasks;";

                var dataTable = DatabaseConnection.ExecuteQuery(query);
                var tasks = new List<DevTask>();

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        var task = new DevTask(
                            id : row["Id"].ToString(),
                            title : row["Title"].ToString(),
                            description : row["Description"].ToString(),
                            techLeaderLogin : row["TechLeaderLogin"].ToString(),
                            developerLogin : row["DeveloperLogin"].ToString(),
                            status : Enum.Parse<StatusEnum>(row["Status"].ToString()),
                            requiresApprovalToComplete: row["RequiresApprovalToComplete"].ToString().Equals("1"),
                            deadline: string.IsNullOrEmpty(row["Deadline"].ToString()) ? DateTime.MinValue : DateTime.Parse(row["Deadline"].ToString()),
                            completionDateTime: string.IsNullOrEmpty(row["CompletionDateTime"].ToString()) ? DateTime.MinValue : DateTime.Parse(row["CompletionDateTime"].ToString())

                        );

                        tasks.Add(task);
                    }
                }

                return tasks;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro obtendo lista de DevTasks: {ex.Message}");
                return new List<DevTask>();
            }
        }
        public static void DisplayAll()
        {
            try
            {
                string query = "SELECT Id, TechLeaderLogin, Title, Deadline, DeveloperLogin FROM DevTasks;";
                var dataTable = DatabaseConnection.ExecuteQuery(query);

                Console.Clear();
                Title.AllTasks();
                Console.WriteLine();

                if (dataTable != null)
                {
                    PrintTasks(dataTable);
                }
                else
                {
                    Console.WriteLine("Erro ao capturar data da tabela DevTasks.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro ao apresentar Tasks: {ex.Message}");
            }
        }
        private static void PrintTasks(DataTable tasksData)
        {
            Console.WriteLine("{0,-5} {1,-20} {2,-50} {3,-20} {4,-20}", "Id", "TechLeaderLogin", "Title", "Deadline", "DeveloperLogin");

            foreach (DataRow row in tasksData.Rows)
            {
                Console.WriteLine("{0,-5} {1,-20} {2,-50} {3,-20} {4,-20}",
                    row["Id"], row["TechLeaderLogin"], row["Title"], row["Deadline"], row["DeveloperLogin"] ?? "N/A");
            }
        }
        private static void PrintTasks(List<DevTask> tasks)
        {
            Title.AllTasks();
            foreach (DevTask task in tasks)
            {
                task.ToStringPrint();
            }
        }
        internal static void DisplayTasksByDeveloper(string login)
        {
            Title.AllTasks();
            Console.WriteLine($"\nDeveloper: {login}");

            List<DevTask> thisDevTaskList =
                GetTaskList()
                .Where(user => user.DeveloperLogin == login)
                .OrderBy(task => task.Status == StatusEnum.Cancelada ? 1 : 0) //cancelada vem ao final
                .ToList();

            PrintTasks(thisDevTaskList);
            Message.PressAnyKeyToContinue();
        }
        internal static void DisplayTasksByTeam(string login)
        {
            Title.AllTasks();
            Console.WriteLine($"\nTech Leader: {login}");

            List<DevTask> thisDevTaskList = 
                GetTaskList()
                .Where(user => user.TechLeaderLogin == login)
                .OrderBy(task => task.Status == StatusEnum.Cancelada ? 1 : 0) //cancelada vem ao final
                .ToList();

            PrintTasks(thisDevTaskList);
            Message.PressAnyKeyToContinue();
        }

        // update methods

        internal static void CancelTaskById(DevTask task, User techLeader)
        {
            try
            {
                const string updateStatusToCancelledQuery = "UPDATE DevTasks SET Status = @Status WHERE Id = @Id AND TechLeaderLogin = @TechLeaderLogin;";

                var parameters = new Dictionary<string, object>
                {
                    { "@Id", task.Id },
                    { "@TechLeaderLogin", techLeader.Login },
                    { "@Status", task.Status.ToString()}
                };

                DatabaseConnection.ExecuteNonQuery(updateStatusToCancelledQuery, parameters);
                Console.WriteLine($"Status alterado para Cancelado na tarefa '{task.Title}' (ID: {task.Id}).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro alterando status da tarefa: {ex.Message}");
            }
        }

        internal static void ApproveTaskById(DevTask? taskToApprove, User techLeader)
        {
            try
            {
                const string updateStatusToCancelledQuery = "UPDATE DevTasks SET RequiresApprovalToComplete = @RequiresApprovalToComplete WHERE Id = @Id AND TechLeaderLogin = @TechLeaderLogin;";

                var parameters = new Dictionary<string, object>
                {
                    { "@Id", taskToApprove.Id },
                    { "@RequiresApprovalToComplete", taskToApprove.RequiresApprovalToComplete.ToString() },
                    { "@TechLeaderLogin", techLeader.Login }
                };

                DatabaseConnection.ExecuteNonQuery(updateStatusToCancelledQuery, parameters);
                Console.WriteLine($"RequiresApprovalToComplete alterado para False na tarefa '{taskToApprove.Title}' (ID: {taskToApprove.Id}).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro alterando RequiresApprovalToComplete da tarefa: {ex.Message}");
            }
        }
    }
}
