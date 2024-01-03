using System.Reflection;
using TaskManager.ConsoleInteraction;
using TaskManager.DomainLayer.Infrastructure;

namespace TaskManager.DomainLayer.Service.CustomMenu
{
    public class UserService
    {
        private readonly MainMenu _menuService;

        public UserService()
        {
            _menuService = new MainMenu();
        }
        public void Run()
        {
            while (true)
            {
                int userSelection = _menuService.DisplayMainMenu();
                _menuService.AnalyzeMainMenu(userSelection);
                Menu.PressAnyKeyToReturn();
            }
        }
    }
}
