using TaskManager.DomainLayer.Service.CustomMenu;
using TaskManager.DomainLayer.Service.Database;
using TaskManager.ConsoleInteraction.Components;

namespace TaskManager.UI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Title.DatabaseInformation();
                DatabaseSetup.Execute();

                var userService = new UserService();
                userService.Run();
            }
            catch (Exception ex)
            {
                Title.Error();
                Console.WriteLine($"\n{ex.InnerException}");
                Main(args);
            }
        }
    }
}
