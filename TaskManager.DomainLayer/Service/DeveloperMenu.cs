using TaskManager.ConsoleInteraction.Components;
using TaskManager.ConsoleInteraction;
using TaskManager.DomainLayer.Model.People;

namespace TaskManager.DomainLayer.Service
{
    internal class DeveloperMenu
    {
        private readonly Menu _developerMenu;
        private readonly User _developer;

        public DeveloperMenu(User developer)
        {
            _developer = developer;
            string[] developerMenuOptions = { "Alterar senha", "Sair" };
            _developerMenu = new Menu(developerMenuOptions);
        }

        public void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                int selectedOption = _developerMenu.DisplayMenu(Title.GreetingDev());
                if (!AnalyzeMainMenu(selectedOption))
                {
                    break;
                }
            }
        }

        private bool AnalyzeMainMenu(int selectedOption)
        {
            switch (selectedOption)
            {
                case 0:
                    _developer.TryChangingPassword();
                    return true;
                case 1:
                    Message.Returning();
                    return false;
                default:
                    Console.WriteLine("Opção inválida");
                    return true;
            }
        }
    }
}
