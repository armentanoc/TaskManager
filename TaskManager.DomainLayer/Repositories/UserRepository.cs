
using TaskManager.DomainLayer.Model;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Service;

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
                else
                {
                    userList = JSONReader.Execute(userList, fullPath);
                    DisplayAll();
                }
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
