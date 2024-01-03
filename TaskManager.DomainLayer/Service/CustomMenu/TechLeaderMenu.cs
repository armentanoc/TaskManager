
using TaskManager.ConsoleInteraction.Components;
using TaskManager.ConsoleInteraction;
using TaskManager.DomainLayer.Repositories;
using TaskManager.DomainLayer.Model.People;
using TaskManager.DomainLayer.Service.DevTaskHelper;

namespace TaskManager.DomainLayer.Service.CustomMenu
{
    internal class TechLeaderMenuService
    {
        private readonly Menu _techLeaderMenu;
        private readonly User _techLeader;

        public TechLeaderMenuService(User techLeader)
        {
            _techLeader = techLeader;
            string[] techLeaderMenuOptions = { "Alterar senha", "Add novos devs via JSON", "Minhas tarefas", "Tarefas do time", "Aprovar tarefa", "Cancelar tarefa", "Sair" };
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
                    return true;
                case 2:
                    DevTaskRepository.DisplayTasksByDeveloper(_techLeader.Login);
                    return true;
                case 3:
                    DevTaskRepository.DisplayTasksByTeam(_techLeader.Login);
                    return true;
                case 4:
                    ApproveTask.Execute(_techLeader);
                    return true;
                case 5:
                    CancelTask.Execute(_techLeader);
                    return true;
                case 6:
                    Message.Returning();
                    return false;
                default:
                    Console.WriteLine("Opção inválida");
                    return true;
            }
        }
    }
}
