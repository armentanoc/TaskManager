
using TaskManager.ConsoleInteraction;

namespace TaskManager.DomainLayer.Service
{
    public class UserService
    {
        private readonly MenuService _menuService;

        public UserService()
        {
            _menuService = new MenuService();
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
