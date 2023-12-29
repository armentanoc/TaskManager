
using TaskManager.ConsoleInteraction.Components;
using TaskManager.ConsoleInteraction;
using TaskManager.DomainLayer.Model;
using TaskManager.DomainLayer.Repositories;

namespace TaskManager.DomainLayer.Service
{
    internal class TechLeaderMenuService
    {
        private readonly Menu _techLeaderMenu;
        private readonly User _techLeader;

        public TechLeaderMenuService(User techLeader)
        {
            _techLeader = techLeader;
            string[] techLeaderMenuOptions = { "Alterar senha", "Adicionar novos desenvolvedores via JSON", "Sair" };
            _techLeaderMenu = new Menu(techLeaderMenuOptions);
        }

        public void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                int selectedOption = _techLeaderMenu.DisplayMenu(Title.GreetingTechLead());
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
                    _techLeader.TryChangingPassword();
                    return true;
                case 1:
                    string relativePath = Message.AskForJSONPath();
                    UserRepository.AddUsersFromJson(relativePath);
                    return false;
                case 2:
                    Message.Returning();
                    return false;
                default:
                    Console.WriteLine("Opção inválida");
                    return true;
            }
        }
    }
}
