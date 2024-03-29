﻿
using System.Data;
using System.Data.SQLite;

using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Files;
using TaskManager.DomainLayer.Infrastructure.Operations;
using TaskManager.DomainLayer.Infrastructure.Operations.DevTaskRepositoryOperations;
using TaskManager.DomainLayer.Model.People;
using TaskManager.DomainLayer.Model.Tasks;

namespace TaskManager.DomainLayer.Infrastructure.Repositories
{
    internal static class DevTaskRepo
    {
        private const string TableName = "DevTasks";

        // initialize methods
        internal static void Initialize()
        {
            using (var connection = DatabaseConnection.CreateConnection("inicializar tabela DevTasks"))
            {
                Create.Table(connection, TableName);
                DatabaseConnection.CloseConnection(connection, "inicializar tabela DevTasks");
            }

            InitializeDefaultTasks();
        }
        private static void InitializeDefaultTasks()
        {
            SQLiteConnection? defaultConnection = null;

            try
            {
                Message.InitializeDefaultDevTasks();

                defaultConnection = DatabaseConnection.CreateConnection("inserir tarefas padrão em DevTasks");
                var tasks = GetTaskList();

                if (tasks.Count == 0)
                {
                    DefaultDevTasks.InsertDefaultTasks(defaultConnection);
                }
            }
            finally
            {
                DatabaseConnection.CloseConnection(defaultConnection, "inserir tarefas padrão em DevTasks");
            }
        }

        internal static void InitializeNewDevTask(DevTask newTask)
        {
            SQLiteConnection? defaultConnection = null;

            try
            {
                Message.InitializeDefaultDevTasks();

                defaultConnection = DatabaseConnection.CreateConnection("inserir nova tarefa em DevTasks");
                var tasks = GetTaskList();

                InsertTaskIfNotExists(defaultConnection, newTask);
            }
            catch (Exception ex)
            {
                Message.CatchException(ex);
            }
            finally
            {
                DatabaseConnection.CloseConnection(defaultConnection, "inserir nova tarefa em DevTasks");
            }
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
                    Message.Error($"Erro executando transação: {ex.Message}");
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
                else
                {
                    throw new Exception($"DevTask com título {task.Title} e descrição {task.Description} já existe.");
                }
            }
            catch (SQLiteException ex)
            {
                Message.Error($"Erro ao inserir as tarefas DevTasks na tabela: {ex.Message}");
            }
            catch (Exception ex)
            {
                Message.Error($"Erro ao inserir as tarefas DevTasks na tabela: {ex.Message}");
            }
        }
        private static void InsertTask(DevTask task)
        {
            try
            {
                string insertTaskQuery = $@"
                    INSERT INTO {TableName} (Title, Description, TechLeaderLogin, DeveloperLogin, Status, RequiresApprovalToComplete, Deadline, CompletionDateTime)
                    VALUES (@Title, @Description, @TechLeaderLogin, @DeveloperLogin, @Status, @RequiresApprovalToComplete, @Deadline, @CompletionDateTime);";

                var parameters = new Dictionary<string, object?>
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
                Message.LogAndConsoleWrite($"Task '{task.Title}' inserida com sucesso na tabela.");
                Message.PressAnyKeyToContinue();
            }
            catch (SQLiteException ex)
            {
                Message.Error($"Erro ao inserir DevTask na tabela: {ex.Message}");
            }
        }

        // validation methods
        internal static bool DoesTaskExist(SQLiteConnection connection, DevTask task)
        {
            string query = $"SELECT COUNT(*) FROM {TableName} WHERE Title = @Title AND Description = @Description;";

            var parameters = new Dictionary<string, object?>
                {
                    { "@Title", task.Title },
                    { "@Description", task.Description }
                };

            int count = Convert.ToInt32(DatabaseConnection.ExecuteScalar(connection, query, parameters));

            return count > 0;

        }
        internal static bool DoesTaskExist(string taskId, User user)
        {
            string operation = "checar se tarefa existe para relacionamento";

            using (var connection = DatabaseConnection.CreateConnection(operation))
            {
                string query = $"SELECT COUNT(*) FROM DevTasks WHERE Id = @Id AND TechLeaderLogin = @TechLeaderLogin;";

                var parameters = new Dictionary<string, object?>
                    {
                        { "@Id", taskId },
                        { "@TechLeaderLogin", user.Login }
                    };

                int count = Convert.ToInt32(DatabaseConnection.ExecuteScalar(connection, query, parameters));
                DatabaseConnection.CloseConnection(connection, operation);
                return count > 0;
            }
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
                    foreach (DataRow? row in dataTable.Rows)
                    {
                        var task = CreateDevTaskFromDataRow(row);
                        tasks.Add(task);
                    }
                }

                return tasks;
            }
            catch (Exception ex)
            {
                Message.Error($"Error obtaining the list of {TableName}: {ex.Message}");
                return new List<DevTask>();
            }
        }

        private static DevTask CreateDevTaskFromDataRow(DataRow? row)
        {
            return new DevTask(
                id: row["Id"].ToString(),
                title: row["Title"].ToString(),
                description: row["Description"].ToString(),
                techLeaderLogin: row["TechLeaderLogin"].ToString(),
                developerLogin: row["DeveloperLogin"].ToString(),
                status: Enum.Parse<StatusEnum>(row["Status"].ToString()),
                requiresApprovalToComplete: row["RequiresApprovalToComplete"].ToString().Equals("1"),
                deadline: ParseDateTime(row["Deadline"].ToString()),
                completionDateTime: ParseDateTime(row["CompletionDateTime"].ToString())
            );
        }

        private static DateTime ParseDateTime(string dateTimeString)
        {
            if (string.IsNullOrEmpty(dateTimeString))
            {
                return DateTime.MinValue;
            }

            return DateTime.Parse(dateTimeString);
        }

        public static void DisplayAll()
        {
            PrintTasks(GetTaskList());
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
            Console.Clear();
            Title.AllTasks();
            foreach (DevTask task in tasks)
            {
                task.ToStringPrint();
            }
        }

        // display tasks methods
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
        internal static void DisplayRelatedTasksByDeveloper(string login)
        {
            Title.AllTasks();
            Console.WriteLine($"\nDeveloper: {login}");
            var thisDevTaskList = GetDevTaskList(login);
            var relationships = GetRelationships(thisDevTaskList);
            var allRelatedTasks = GetRelatedTasks(login, relationships);
            PrintTasks(allRelatedTasks);
            Message.PressAnyKeyToContinue();
        }
        internal static void DisplayRelatedTasksByTechLeader(string login)
        {
            Title.AllTasks();
            Console.WriteLine($"\nTechLeader: {login}");
            var thisDevTaskList = GetTeamTaskList(login);
            var relationships = GetRelationships(thisDevTaskList);
            var allRelatedTasks = GetRelatedTasks(login, relationships);
            PrintTasks(allRelatedTasks);
            Message.PressAnyKeyToContinue();
        }
        internal static List<DevTask> GetRelatedTasks(string login, IEnumerable<DevTaskRelationship> relationships)
        {
            var allRelatedTasks = new List<DevTask>();

            foreach (var relation in relationships)
            {
                var relatedTasks = GetRelatedIndividualTask(relation, login);
                allRelatedTasks.AddRange(relatedTasks);
            }

            return allRelatedTasks;
        }
        internal static IEnumerable<DevTask> GetRelatedIndividualTask(DevTaskRelationship relation, string login)
        {
            return GetTaskList()
                .Where(
                    task =>
                        (task.Id.Equals(relation.ParentOrFirstTaskId) || task.Id.Equals(relation.ChildOrSecondTaskId))
                        && !task.DeveloperLogin.Equals(login)
                )
                .ToList();
        }
        internal static IEnumerable<DevTaskRelationship> GetRelationships(IEnumerable<DevTask> thisDevTaskList)
        {
            List<DevTaskRelationship> relationships = new List<DevTaskRelationship>();

            foreach (var task in thisDevTaskList)
            {
                var relationsOfThisTask = GetRelationsOfThisTask(task);

                relationships.AddRange(relationsOfThisTask);
            }

            return relationships;
        }
        internal static IEnumerable<DevTaskRelationship> GetRelationsOfThisTask(DevTask task)
        {
            return DevTaskRelationshipRepo.GetDevTaskRelationshipsList()
                .Where(relation => relation.ParentOrFirstTaskId.Equals(task.Id) || relation.ChildOrSecondTaskId.Equals(task.Id))
                .ToList();
        }
        internal static IEnumerable<DevTask> GetDevTaskList(string login)
        {
            return GetTaskList()
                .Where(task => task.DeveloperLogin.Equals(login))
                .ToList();
        }
        internal static IEnumerable<DevTask> GetTeamTaskList(string login)
        {
            return GetTaskList()
                .Where(task => task.TechLeaderLogin.Equals(login))
                .ToList();
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
                Message.LogAndConsoleWrite($"Status alterado para Cancelado na tarefa '{task.Title}' (ID: {task.Id}).");
            }
            catch (Exception ex)
            {
                Message.LogAndConsoleWrite($"Erro alterando status da tarefa: {ex.Message}");
            }
        }
        internal static void ApproveTaskById(DevTask? taskToApprove, User techLeader)
        {
            try
            {
                const string updateStatusToCancelledQuery =
                    @"UPDATE DevTasks 
                    SET RequiresApprovalToComplete = @RequiresApprovalToComplete 
                    WHERE 
                         Id = @Id 
                         AND TechLeaderLogin = @TechLeaderLogin;";

                var parameters = new Dictionary<string, object>
                {
                    { "@Id", taskToApprove.Id },
                    { "@RequiresApprovalToComplete", taskToApprove.RequiresApprovalToComplete.ToString() },
                    { "@TechLeaderLogin", techLeader.Login }
                };

                DatabaseConnection.ExecuteNonQuery(updateStatusToCancelledQuery, parameters);
                Message.LogAndConsoleWrite($"RequiresApprovalToComplete alterado para False na tarefa '{taskToApprove.Title}' (ID: {taskToApprove.Id}).");
            }
            catch (Exception ex)
            {
                Message.Error($"Erro alterando RequiresApprovalToComplete da tarefa: {ex.Message}");
            }
        }
        internal static void UpdateTaskStatusById(DevTask? taskToUpdate, User developer)
        {
            try
            {
                const string updateStatusQuery = @"
                       
                        UPDATE DevTasks 
                            SET Status = @Status, 
                            CompletionDateTime = @CompletionDateTime  
                        WHERE Id = @Id 
                        AND DeveloperLogin = @DeveloperLogin;";

                var parameters = new Dictionary<string, object?>
                {
                    { "@Id", taskToUpdate.Id },
                    { "@Status", taskToUpdate.Status.ToString() },
                    { "@DeveloperLogin", developer.Login },
                    { "@CompletionDateTime", taskToUpdate.CompletionDateTime }
                };

                DatabaseConnection.ExecuteNonQuery(updateStatusQuery, parameters);
                Message.LogAndConsoleWrite($"Status alterado para {taskToUpdate.Status} na tarefa '{taskToUpdate.Title}' (ID: {taskToUpdate.Id}).");
            }
            catch (Exception ex)
            {
                Message.Error($"Erro alterando Status da tarefa: {ex.Message}. Obs.: Verificar se a tarefa já foi aprovada (boolean RequiresApprovalToComplete = false)");
            }
        }
        internal static void UpdateTaskStatusByIdFromTechLeader(DevTask? taskToUpdate, User developer)
        {
            try
            {
                const string updateStatusQuery = @"
                       
                        UPDATE DevTasks 
                            SET Status = @Status, 
                            RequiresApprovalToComplete = @RequiresApprovalToComplete,  
                            CompletionDateTime = @CompletionDateTime  
                        WHERE Id = @Id 
                        AND TechLeaderLogin = @TechLeaderLogin;";

                var parameters = new Dictionary<string, object?>
                {
                    { "@Id", taskToUpdate.Id },
                    { "@Status", taskToUpdate.Status.ToString() },
                    { "@RequiresApprovalToComplete", taskToUpdate.RequiresApprovalToComplete },
                    { "@TechLeaderLogin", developer.Login },
                    { "@CompletionDateTime", taskToUpdate.CompletionDateTime }
                };

                DatabaseConnection.ExecuteNonQuery(updateStatusQuery, parameters);
                Message.LogAndConsoleWrite($"Status alterado para {taskToUpdate.Status} na tarefa '{taskToUpdate.Title}' (ID: {taskToUpdate.Id}).");
            }
            catch (Exception ex)
            {
                Message.Error($"Erro alterando Status da tarefa: {ex.Message}. Obs.: Verificar se a tarefa já foi aprovada (boolean RequiresApprovalToComplete = false) ou se você é o líder técnico responsável.");
            }
        }
        internal static void UpdateTaskDeadlineById(DevTask? taskToUpdate, User techLeader)
        {
            try
            {
                const string updateStatusQuery = @"
                       
                        UPDATE DevTasks 
                            SET Deadline = @Deadline
                        WHERE Id = @Id 
                        AND TechLeaderLogin = @TechLeaderLogin;";

                var parameters = new Dictionary<string, object>
                {
                    { "@Id", taskToUpdate.Id },
                    { "@Deadline", taskToUpdate.Deadline },
                    { "@TechLeaderLogin", techLeader.Login }
                };

                DatabaseConnection.ExecuteNonQuery(updateStatusQuery, parameters);
                Message.LogAndConsoleWrite($"Deadline alterada para {taskToUpdate.Deadline} na tarefa '{taskToUpdate.Title}' (ID: {taskToUpdate.Id}).");
            }
            catch (Exception ex)
            {
                Message.Error($"Erro alterando Deadline da tarefa: {ex.Message}. ");
            }
        }
        internal static void UpdateTaskDeveloperLoginById(DevTask taskToAlter, string developerLogin, string techLeaderLogin)
        {
            try
            {
                const string updateStatusQuery = @"
                       
                        UPDATE DevTasks 
                            SET DeveloperLogin = @DeveloperLogin
                        WHERE Id = @Id 
                        AND TechLeaderLogin = @TechLeaderLogin;";

                var parameters = new Dictionary<string, object>
                {
                    { "@Id", taskToAlter.Id },
                    { "@DeveloperLogin", developerLogin },
                    { "@TechLeaderLogin", techLeaderLogin }
                };

                DatabaseConnection.ExecuteNonQuery(updateStatusQuery, parameters);
                Message.LogAndConsoleWrite($"Dev da tarefa '{taskToAlter.Title}' (ID {taskToAlter.Id}) alterado para {developerLogin}.");
            }
            catch (Exception ex)
            {
                Message.Error($"Erro alterando Deadline da tarefa: {ex.Message}. ");
            }
        }
    }
}

