
using System.Data.Entity;
using System.Data.SQLite;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Model.People;
using TaskManager.DomainLayer.Service;
using TaskManager.Infrastructure.Operations;

namespace TaskManager.Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private const string TableName = "Users";
        private static readonly object lockObject = new object();
        public static List<User> userList = new List<User>();

        internal static void Initialize()
        {
            using (var connection = DatabaseConnection.CreateConnection())
            {
                CreateUsersTable(connection);
                InsertDefaultUsersIntoTable(connection);
                DatabaseConnection.CloseConnection(connection, "inicializar tabela de usuários");
            }
        }
        private static void InsertDefaultUsersIntoTable(SQLiteConnection connection)
        {
            userList = new List<User>
            {
                new TechLeader("Kaio", "kaio"),
                new Developer("Ana Carolina", "carolina"),
            };

            InsertUsersIfNotExist(connection, userList);
        }

        private static void InsertUsersIfNotExist(SQLiteConnection connection, List<User> users)
        {
            try
            {
                foreach (var user in users)
                {
                    if (!DoesUserExist(connection, user))
                    {
                        InsertUser(connection, user);
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Erro ao inserir usuários na tabela: {ex.Message}");
            }
        }

        private static void CreateUsersTable(SQLiteConnection connection)
        {
            const string createUsersQuery = $@"
                    CREATE TABLE IF NOT EXISTS {TableName} (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Login TEXT NOT NULL,
                        Password TEXT NOT NULL,
                        Email TEXT,
                        JobType TEXT NOT NULL
                    );";

            Tables.Create(connection, TableName, createUsersQuery);
        }

        private static void InsertUser(SQLiteConnection connection, User user)
        {
            try
            {
                string insertUserQuery = $@"
                    INSERT INTO {TableName} (Name, Login, Password, Email, JobType)
                    VALUES (@Name, @Login, @Password, @Email, @JobType);";

                var parameters = new Dictionary<string, object>
                {
                    { "@Name", user.Name },
                    { "@Login", user.Login },
                    { "@Password", user.Password },
                    { "@Email", user.Email },
                    { "@JobType", user.Job}
                };

                DatabaseConnection.ExecuteNonQuery(connection, insertUserQuery, parameters);
                Console.WriteLine($"Usuário {user.Name} inserido com sucesso na tabela.");
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Erro ao inserir usuário na tabela: {ex.Message}");
            }
        }

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

        public static void AddUser(User user)
        {
            lock (lockObject)
            {
                DatabaseConnection.ExecuteWithinTransaction((connection) =>
                {
                    InsertUser(connection, user);
                });
            }
        }

        public static void AddUsersFromJson(string filePath)
        {
            try
            {
                string domainLayerDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "TaskManager.DomainLayer", "Files", filePath);
                string fullPath = Path.GetFullPath(domainLayerDirectory);

                if (!File.Exists(fullPath))
                {
                    Console.WriteLine("Arquivo JSON não encontrado. Por favor, verifique o caminho.");
                    return;
                }
                else
                {
                    userList = JSONReader.Execute(userList, fullPath);

                    DatabaseConnection.ExecuteWithinTransaction((connection) =>
                    {
                        InsertUsersIfNotExist(connection, userList);
                    });

                    DisplayAll();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar usuários do JSON: {ex.Message}");
            }
        }

        public static List<User> All()
        {
            return userList;
        }

        public static void DisplayAll()
        {
            Console.Clear();
            Title.AllUsers();
            try
            {
                foreach (var user in userList)
                {
                    Console.WriteLine(user.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro ao exibir usuários: {ex.Message}");
            }
        }
        public User GetUserByLogin(string login)
        {
            throw new NotImplementedException();
        }
    }
}
