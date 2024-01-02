
using System.Data;
using System.Data.SQLite;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Model.Tasks;
using TaskManager.DomainLayer.Operations;
using TaskManager.DomainLayer.Service.Database.Operations;

namespace TaskManager.DomainLayer.Repositories
{
    internal static class DevTaskRepository
    {
        private const string TableName = "DevTasks";
        private static readonly object lockObject = new object();
        public static List<DevTask> taskList = new List<DevTask>();

        internal static void Initialize()
        {
            try
            {
                using (var connection = DatabaseConnection.CreateConnection())
                {
                    CreateDevTasksTable(connection);
                    InitializeDefaultTasks();
                    InsertDefaultTasksIntoTable(connection);
                    DatabaseConnection.CloseConnection(connection, "inicializar tabela DevTasks");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inicializar o DevTaskRepository: {ex.Message}");
            }
        }

        private static void InitializeDefaultTasks()
        {
            taskList.Add(new DevTask(
                techLeaderLogin: "kaio",
                title: "Implementar Funcionalidade X",
                deadline: DateTime.Now.AddDays(7),
                developerLogin: "kaio"
            ));

            taskList.Add(new DevTask(
                techLeaderLogin: "kaio",
                title: "Corrigir bug Y",
                deadline: DateTime.Now.AddDays(3),
                developerLogin: "carolina"
            ));

            taskList.Add(new DevTask(
                techLeaderLogin: "kaio",
                title: "Implementar Funcionalidade Z",
                deadline: DateTime.Now.AddDays(10)
            ));
        }
        private static void CreateDevTasksTable(SQLiteConnection connection)
        {
            const string createDevTasksQuery = $@"
                    CREATE TABLE {TableName} (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    TechLeaderLogin TEXT NOT NULL,
                    Title TEXT NOT NULL,
                    Deadline DATETIME NOT NULL,
                    DeveloperLogin TEXT,
                    FOREIGN KEY (TechLeaderLogin) REFERENCES Users(Login),
                    FOREIGN KEY (DeveloperLogin) REFERENCES Users(Login)
                    )";

            Tables.Create(connection, TableName, createDevTasksQuery);
        }
        private static void InsertDefaultTasksIntoTable(SQLiteConnection connection)
        {
            InsertTasksIfNotExist(connection, taskList);
        }
        private static void InsertTasksIfNotExist(SQLiteConnection connection, List<DevTask> tasks)
        {
            try
            {
                foreach (var task in tasks)
                {
                    if (!DoesTaskExist(connection, task))
                    {
                        InsertTask(connection, task);
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Erro ao inserir as tarefas DevTasks na tabela: {ex.Message}");
            }
        }
        private static void InsertTask(SQLiteConnection connection, DevTask task)
        {
            try
            {
                string insertTaskQuery = $@"
                    INSERT INTO {TableName} (TechLeaderLogin, Title, Deadline, DeveloperLogin)
                    VALUES (@TechLeaderLogin, @Title, @Deadline, @DeveloperLogin);";

                var parameters = new Dictionary<string, object>
                {
                    { "@TechLeaderLogin", task.TechLeaderLogin },
                    { "@Title", task.Title },
                    { "@Deadline", task.Deadline },
                    { "@DeveloperLogin", task.DeveloperLogin },
                };

                DatabaseConnection.ExecuteNonQuery(insertTaskQuery, parameters);
                Console.WriteLine($"Task '{task.Title}' inserida com sucesso na tabela.");
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Erro ao inserir DevTask na tabela: {ex.Message}");
            }
        }
        private static bool DoesTaskExist(SQLiteConnection connection, DevTask task)
        {
            string query = $"SELECT COUNT(*) FROM {TableName} WHERE Title = @Title;";

            var parameters = new Dictionary<string, object>
            {
                { "@Title", task.Title },
            };

            int count = Convert.ToInt32(DatabaseConnection.ExecuteScalar(connection, query, parameters));

            return count > 0;
        }
        public static void AddDevTask(DevTask task)
        {
            lock (lockObject)
            {
                DatabaseConnection.ExecuteWithinTransaction((connection) =>
                {
                    InsertTask(connection, task);
                });
            }
        }
        public static void DisplayAll()
        {
            Console.Clear();
            Title.AllTasks();
            try
            {
                string query = "SELECT Id, TechLeaderLogin, Title, Deadline, DeveloperLogin FROM DevTasks;";
                var dataTable = DatabaseConnection.ExecuteQuery(query);

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

    }
}
