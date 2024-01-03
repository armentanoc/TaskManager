using TaskManager.ConsoleInteraction.Components;
using TaskManager.ConsoleInteraction;
using TaskManager.DomainLayer.Model.People;
using TaskManager.DomainLayer.Service.DevTaskHelper;
using TaskManager.DomainLayer.Infrastructure.Repositories;

namespace TaskManager.DomainLayer.Service.CustomMenu
{
    internal class DeveloperMenu
    {
        private readonly Menu _developerMenu;
        private readonly User _developer;

        public DeveloperMenu(User developer)
        {
            _developer = developer;

            string[] developerMenuOptions = 
                { 
                "Alterar senha", 
                "Criar tarefa", 
                "Minhas tarefas", 
                "Modificar status de tarefa", 
                "Sair" 
            };

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
                    CreateDevTask.Execute(_developer);
                    return true;
                case 2:
                    DevTaskRepository.DisplayTasksByDeveloper(_developer.Login);
                    return true;
                case 3:
                    UpdateTaskStatus.Execute(_developer);
                    return true;
                case 4:
                    Message.Returning();
                    return false;
                default:
                    Console.WriteLine("Opção inválida");
                    return true;
            }
        }
    }
}
