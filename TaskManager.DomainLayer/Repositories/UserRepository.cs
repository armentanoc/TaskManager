using TaskManager.DomainLayer.Model;

namespace TaskManager.DomainLayer.Repositories
{
    internal static class UserRepository
    {
        public static List<User> userList = new List<User>
        {
            new Developer("Ana Carolina", "carolina", "123"),
            new Developer("Ronily", "roni", "123"),
            new Developer("Vanessa", "van", "123"),
            new TechLeader("Kaio", "kaio", "123"),
        };

        public static List<User> All()
        {
            return userList;
        }

        public static void DisplayAll()
        {
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
