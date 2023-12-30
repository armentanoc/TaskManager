using TaskManager.DomainLayer.Service;
using TaskManager.ConsoleInteraction.Components;

namespace TaskManager.UI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var userService = new UserService();
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
