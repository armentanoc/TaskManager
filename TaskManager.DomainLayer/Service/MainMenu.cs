
using TaskManager.ConsoleInteraction.Components;
using TaskManager.ConsoleInteraction;
using TaskManager.DomainLayer.Repositories;
using TaskManager.DomainLayer.Model.People;

namespace TaskManager.DomainLayer.Service
{
    internal class MainMenu
    {
        private readonly Menu _mainMenu;

        public MainMenu()
        {
            string[] mainMenuOptions = { "Realizar Login", "Usuários", "Tarefas", "Sair" };
            _mainMenu = new Menu(mainMenuOptions);
        }

        public int DisplayMainMenu()
        {
            string title = Title.MainMenu();
            return _mainMenu.DisplayMenu(title);
        }

        public void AnalyzeMainMenu(int selectedOption)
        {
            switch (selectedOption)
            {
                case 0:
                    User user = Authentication.Authenticate(UserRepository.All());
                    break;
                case 1:
                    UserRepository.DisplayAll();
                    break;
                case 2:
                    DevTaskRepository.DisplayAll();
                    break;
                case 3:
                    Title.EnvironmentExit();
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Opção inválida");
                    break;
            }
        }
    }
}
