using System.Text.Json;
using TaskManager.DomainLayer.Model;
using TaskManager.ConsoleInteraction.Components;

namespace TaskManager.DomainLayer.Repositories
{
    internal static class UserRepository
    {
        public static List<User> userList;

        static UserRepository()
        {
            InitializeDefaultUsers();
        }
        private static void InitializeDefaultUsers()
        {
            userList = new List<User>
            {
                new Developer("Ana Carolina", "carolina"),
                new TechLeader("Kaio", "kaio"),
            };
        }
        internal static void AddUsersFromJson(string filePath)
        {
            try
            {
                string domainLayerDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "TaskManager.DomainLayer", "Files", filePath);
                string fullPath = Path.GetFullPath(domainLayerDirectory);

                if (!File.Exists(fullPath))
                {
                    Console.WriteLine("Arquivo JSON não encontrado. Verifique o caminho.");
                    return;
                }

                string jsonData = File.ReadAllText(fullPath);
                var userDTOs = JsonSerializer.Deserialize<List<UserDTO>>(jsonData);

                foreach (var userDTO in userDTOs)
                {
                    User existingUser = userList.FirstOrDefault(u => u.Name == userDTO.Name || u.Login == userDTO.Login);

                    if (existingUser != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\nAtenção: Usuário com nome '{userDTO.Name}' ou login '{userDTO.Login}' já existe. Pulando.");
                        Console.ResetColor();
                        continue;
                    }

                    User user;

                    if (!Enum.TryParse(userDTO.Job, out JobEnum job))
                    {
                        Console.WriteLine($"Tipo de trabalho inválido: {userDTO.Job}");
                        continue;
                    }

                    switch (userDTO.Job)
                    {
                        case "Developer":
                            user = new Developer(userDTO.Name, userDTO.Login, userDTO.Email);
                            break;

                        case "TechLeader":
                            user = new TechLeader(userDTO.Name, userDTO.Login, userDTO.Email);
                            break;

                        default:
                            Console.WriteLine($"Tipo de usuário desconhecido: {userDTO.Job}");
                            continue;
                    }
                    user.SetJob(job);
                    userList.Add(user);
                }
                Message.NewUsersInUserList();
                DisplayAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inicializar usuários a partir do JSON: {ex.Message}");
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
                Console.WriteLine($"\nErro ao exibir os usuários: {ex.Message}");
            }
        }
    }
}
