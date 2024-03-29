﻿
using System.Data;
using System.Data.SQLite;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Infrastructure.Operations;
using TaskManager.DomainLayer.Model.People;
using TaskManager.DomainLayer.Infrastructure.Operations.UserRepositoryOperations;
using TaskManager.DomainLayer.Files;

namespace TaskManager.DomainLayer.Infrastructure.Repositories
{
    internal class UserRepo
    {
        private const string TableName = "Users";
        private static List<User> userList = new List<User>();

        internal static void Initialize()
        {
            using (var connection = DatabaseConnection.CreateConnection("inicializar tabela de usuários"))
            {
                Create.Table(connection, TableName);
                DefaultUsers.InsertDefaultUsersIntoTable(connection);
                DatabaseConnection.CloseConnection(connection, "inicializar tabela de usuários");
            }
        }

        //insert methods
        internal static void InsertUsersIfNotExist(SQLiteConnection connection, List<User> users, bool isFirstRun = true)
        {
            try
            {
                foreach (var user in users)
                {
                    if (!DoesUserExist(connection, user))
                    {
                        InsertUser(user);
                    }
                    else if (!isFirstRun)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Message.LogAndConsoleWrite($"Obs: User de nome {user.Name} ou login {user.Login} já existe. Pulando.");
                        Console.ResetColor();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Message.LogAndConsoleWrite($"Erro ao inserir usuários na tabela: {ex.Message}");
            }
        }
        private static void InsertUser(User user)
        {
            try
            {
                string insertUserQuery = $@"
                    INSERT INTO {TableName} (Name, Login, Password, Email, JobType)
                    VALUES (@Name, @Login, @Password, @Email, @JobType);";

                var parameters = new Dictionary<string, object?>
                {
                    { "@Name", user.Name },
                    { "@Login", user.Login },
                    { "@Password", user.Password },
                    { "@Email", user.Email },
                    { "@JobType", user.Job.ToString()}
                };

                DatabaseConnection.ExecuteNonQuery(insertUserQuery, parameters);
                Message.LogAndConsoleWrite($"User de nome {user.Name} inserido(a) com sucesso na tabela.");
            }
            catch (SQLiteException ex)
            {
                Message.LogAndConsoleWrite($"Erro ao inserir usuário na tabela: {ex.Message}");
            }
        }
        public static void AddUsersFromJson(string filePath)
        {
            using (var connection = DatabaseConnection.CreateConnection("adicionar usuários via JSON"))
            {
                try
                {
                    string domainLayerDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "TaskManager.DomainLayer", "Files", filePath);
                    string fullPath = Path.GetFullPath(domainLayerDirectory);

                    if (!File.Exists(fullPath))
                    {
                        Message.Error("Arquivo JSON não encontrado. Por gentileza, verifique o path fornecido.");
                        Console.ReadKey();
                        return;
                    }
                    else
                    {
                        userList = JSONReader.Execute(fullPath);
                        InsertUsersIfNotExist(connection, userList, false);
                        Message.NewUsersInUserList();
                        DisplayAll();
                        Message.PressAnyKeyToContinue();
                    }
                }
                catch (Exception ex)
                {
                    Message.Error($"Erro ao adicionar usuários do JSON: {ex.Message}");
                }
            }
        }

        //validation methods
        private static bool DoesUserExist(SQLiteConnection connection, User user)
        {
            string query = $"SELECT COUNT(*) FROM {TableName} WHERE Name = @Name OR Login = @Login;";

            var parameters = new Dictionary<string, object>
            {
                { "@Name", user.Name },
                { "@Login", user.Login }
            };

            int count = Convert.ToInt32(DatabaseConnection.ExecuteScalar(connection, query, parameters));

            return count > 0;
        }

        //read and display methods
        public static void DisplayAll()
        {
            Console.Clear();
            Title.AllUsers();
            PrintUsers(GetUsersList());
        }
        private static void PrintUsers(DataTable usersData)
        {
            Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-15}", "Id", "Name", "Login", "JobType");

            foreach (DataRow row in usersData.Rows)
            {
                Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-15}",
                    row["Id"], row["Name"], row["Login"], row["JobType"]);
            }
        }
        private static void PrintUsers(List<User> users)
        {
            foreach (User user in users)
            {
                Console.WriteLine(user.ToString());
            }
        }

        //get user methods
        public static User? GetUserByLogin(string login)
        {
            try
            {
                string query = $"SELECT * FROM Users WHERE Login = '{login}';";

                var dataTable = DatabaseConnection.ExecuteQuery(query);

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    var row = dataTable.Rows[0];

                    string id = row["Id"].ToString();
                    string name = row["Name"].ToString();
                    string checkedLogin = row["Login"].ToString();
                    string password = row["Password"].ToString();
                    string email = row["Email"].ToString();
                    JobEnum job = Enum.Parse<JobEnum>(row["JobType"].ToString());

                    if (job.Equals(JobEnum.Developer))
                    {
                        return new Developer(id, name, checkedLogin, password, email, job);
                    }
                    else if (job.Equals(JobEnum.TechLeader))
                    {
                        return new TechLeader(id, name, checkedLogin, password, email, job);
                    }
                    else
                    {
                        Console.WriteLine($"JobEnum desconhecido para '{login}'.");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine($"User com login '{login}' não encontrado.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro recuperando usuário: {ex.Message}");
                return null;
            }
        }
        internal static List<User> GetUsersList()
        {
            try
            {
                const string query = "SELECT * FROM Users;";

                var dataTable = DatabaseConnection.ExecuteQuery(query);
                var users = new List<User>();

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow? row in dataTable.Rows)
                    {
                        var user = new User(
                            id: row["Id"].ToString(),
                            name: row["Name"].ToString(),
                            login: row["Login"].ToString(),
                            password: row["Password"].ToString(),
                            email: row["Email"].ToString(),
                            job: Enum.Parse<JobEnum>(row["JobType"].ToString())
                        );

                        users.Add(user);
                    }
                }

                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro obtendo lista de usuários: {ex.Message}");
                return new List<User>();
            }
        }

        //update methods
        internal static void UpdatePasswordById(User user, string newPassword)
        {
            try
            {
                const string updatePasswordQuery = "UPDATE Users SET Password = @Password WHERE Id = @Id;";

                var parameters = new Dictionary<string, object>
                {
                    { "@Id", user.Id },
                    { "@Password", newPassword}
                };

                DatabaseConnection.ExecuteNonQuery(updatePasswordQuery, parameters);
                Message.PasswordChangedSuccessfully($"Senha alterada com sucesso para o User de Login {user.Login}.");
            }
            catch (Exception ex)
            {
                Message.Error($"Erro alterando senha: {ex.Message}");
            }
        }

        internal static void DisplayDevelopers()
        {
            Console.Clear();
            Title.AllUsers();
            PrintUsers(GetDevelopersInDatabase());
        }

        internal static List<User> GetDevelopersInDatabase()
        {
            try
            {
                const string query = "SELECT * FROM Users WHERE JobType = 'Developer';";

                var dataTable = DatabaseConnection.ExecuteQuery(query);
                var developers = new List<User>();

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow? row in dataTable.Rows)
                    {
                        var user = new User(
                            id: row["Id"].ToString(),
                            name: row["Name"].ToString(),
                            login: row["Login"].ToString(),
                            password: row["Password"].ToString(),
                            email: row["Email"].ToString(),
                            job: Enum.Parse<JobEnum>(row["JobType"].ToString())
                        );

                        developers.Add(user);
                    }
                }

                return developers;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro obtendo lista de usuários: {ex.Message}");
                return new List<User>();
            }
        }
    }
}

