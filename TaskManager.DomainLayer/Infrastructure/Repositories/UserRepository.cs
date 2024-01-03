
using System.Data;
using System.Data.SQLite;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Infrastructure.Operations;
using TaskManager.DomainLayer.Model.People;
using TaskManager.DomainLayer.Infrastructure.Operations.UserRepositoryOperations;

namespace TaskManager.DomainLayer.Infrastructure.Repositories
{
    internal class UserRepository
    {
        private const string TableName = "Users";
        private static List<User> userList = new List<User>();

        internal static void Initialize()
        {
            using (var connection = DatabaseConnection.CreateConnection("inicializar tabela de usuários"))
            {
                Create.Table(connection, TableName);
                InsertDefaultUsersIntoTable(connection);
                DatabaseConnection.CloseConnection(connection, "inicializar tabela de usuários");
            }
        }

        //insert methods
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
                        InsertUser(user);
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Erro ao inserir usuários na tabela: {ex.Message}");
            }
        }
        private static void InsertUser(User user)
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
                    { "@JobType", user.Job.ToString()}
                };

                DatabaseConnection.ExecuteNonQuery(insertUserQuery, parameters);
                Console.WriteLine($"Usuário {user.Name} inserido com sucesso na tabela.");
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Erro ao inserir usuário na tabela: {ex.Message}");
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
                        Console.WriteLine("Arquivo JSON não encontrado. Por favor, verifique o caminho.");
                        return;
                    }
                    else
                    {
                        userList = JSONReader.Execute(fullPath);
                        InsertUsersIfNotExist(connection, userList);
                        DisplayAll();
                        Message.PressAnyKeyToContinue();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao adicionar usuários do JSON: {ex.Message}");
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
        public static User GetUserByLogin(string login)
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
                Console.WriteLine($"Error retrieving user: {ex.Message}");
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
                    foreach (DataRow row in dataTable.Rows)
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
                Message.LogAndConsoleWrite($"Senha alterada com sucesso para o User de Login {user.Login}.");
            }
            catch (Exception ex)
            {
                Message.Error($"Erro alterando senha: {ex.Message}");
            }
        }
    }
}
