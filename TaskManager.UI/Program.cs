using TaskManager.DomainLayer.Service.CustomMenu;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Infrastructure;
using System.Reflection;

namespace TaskManager.UI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DatabaseSetup.Execute();
                var userService = new UserService();
                userService.Run();
            }
            catch (Exception ex)
            {
                Message.Error($"\n{ex.InnerException}");
            }
        }
    }
}
