
//using TaskManager.ConsoleInteraction.Components;
//using TaskManager.DomainLayer.Model.People;
//using TaskManager.DomainLayer.Service;
//using TaskManager.Infrastructure.Repositories;

//namespace TaskManager.DomainLayer.Files
//{
//    public class DefaultUsers
//    {
//        public static List<User> userList = new List<User>();

//        private static void InitializeDefaultUsers()
//        {
//            userList = new List<User>
//            {
//                new TechLeader("Henrique", "henrique"),
//                new Developer("Taiane", "taiane"),
//            };
//        }

//        public static void AddDefaultUsersInDatabase()
//        {
//            InitializeDefaultUsers();

//            foreach (var user in userList)
//            {
//                var userDTO = new UserDTOForJson();

//                userDTO.Name = user.Name;
//                userDTO.Email = user.Email;
//                userDTO.Login = user.Login;
//                userDTO.Password = user.Password;
//                userDTO.Job = user.Job.ToString();

//                UserRepository.InsertUserIfNotExist(userDTO);
//                Console.ReadKey();
//            }
//        }
//        public static void AddUsersFromJson(string filePath)
//        {
//            try
//            {
//                string domainLayerDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "TaskManager.DomainLayer", "Files", filePath);
//                string fullPath = Path.GetFullPath(domainLayerDirectory);

//                if (!File.Exists(fullPath))
//                {
//                    Console.WriteLine("Arquivo JSON não encontrado. Por favor, verifique o caminho.");
//                    return;
//                }
//                else
//                {
//                    userList = JSONReader.Execute(userList, fullPath);

//                    //DatabaseConnection.ExecuteWithinTransaction((connection) =>
//                    //{
//                    //    InsertUsersIfNotExist(connection, userList);
//                    //});

//                    DisplayAll();
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Erro ao adicionar usuários do JSON: {ex.Message}");
//            }
//        }

//        public static List<User> All()
//        {
//            return userList;
//        }

//        public static void DisplayAll()
//        {
//            Console.Clear();
//            Title.AllUsers();
//            try
//            {
//                foreach (var user in userList)
//                {
//                    Console.WriteLine(user.ToString());
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"\nErro ao exibir usuários: {ex.Message}");
//            }
//        }

//        //public DefaultUsers()
//        //{
//        //    InsertDefaultUsersIntoTable(connection);
//        //}

//        //private static void InsertDefaultUsersIntoTable(SQLiteConnection connection)
//        //{
//        //    userList = new List<User>
//        //    {
//        //        new TechLeader("Kaio", "kaio"),
//        //        new Developer("Ana Carolina", "carolina"),
//        //    };

//        //    InsertUsersIfNotExist(connection, userList);
//        //}
//    }
//}
