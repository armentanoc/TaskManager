using TaskManager.DomainLayer.Service;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.Infrastructure;

namespace TaskManager.DomainLayer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Title.DatabaseInformation();
                DatabaseSetup.Main();

                var userService = new UserService();
                Console.WriteLine("User service criado.");
                userService.Run();
            }
            catch (Exception ex)
            {
                Title.Error();
                Console.WriteLine($"\n{ex.InnerException}");
            }
        }
    }
}
