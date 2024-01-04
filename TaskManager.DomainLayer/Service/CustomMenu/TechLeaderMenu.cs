
using TaskManager.ConsoleInteraction.Components;
using TaskManager.ConsoleInteraction;
using TaskManager.DomainLayer.Model.People;
using TaskManager.DomainLayer.Infrastructure.Repositories;
using TaskManager.DomainLayer.Service.Relationships;
using TaskManager.DomainLayer.Service.Tasks;

namespace TaskManager.DomainLayer.Service.CustomMenu
{
    internal class TechLeaderMenuService
    {
        private readonly Menu _techLeaderMenu;
        private readonly User _techLeader;

        public TechLeaderMenuService(User techLeader)
        {
            _techLeader = techLeader;

            string[] techLeaderMenuOptions = 
                { 
                "Alterar senha", 
                "Add novos devs via JSON", 
                "Minhas tarefas", 
                "Tarefas do time", 
                "Aprovar tarefa", 
                "Cancelar tarefa", 
                "Criar tarefa", 
                "Criar relacionamento",
                "Modificar status de tarefa",
                "Modificar deadline de tarefa",
                "Sair" 
            };

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
                    UserRepo.AddUsersFromJson(relativePath);
                    return true;
                case 2:
                    DevTaskRepo.DisplayTasksByDeveloper(_techLeader.Login);
                    return true;
                case 3:
                    DevTaskRepo.DisplayTasksByTeam(_techLeader.Login);
                    return true;
                case 4:
                    ApproveTask.Execute(_techLeader);
                    return true;
                case 5:
                    CancelTask.Execute(_techLeader);
                    return true;
                case 6:
                    CreateTask.ExecuteTechLeader(_techLeader);
                    return true;
                case 7:
                    CreateRelationship.Execute(_techLeader);
                    return true;
                case 8:
                    UpdateStatus.ExecuteTechLeader(_techLeader);
                    return true;
                case 9:
                    SetDeadline.Execute(_techLeader);
                    return true;
                case 10:
                    Message.Returning();
                    return false;
                default:
                    Console.WriteLine("Opção inválida");
                    return true;
            }
        }
    }
}
