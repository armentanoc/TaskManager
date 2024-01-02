using TaskManager.ConsoleInteraction.Components;
using TaskManager.ConsoleInteraction;
using TaskManager.DomainLayer.Model.People;
using TaskManager.DomainLayer.Model.Tasks;
using TaskManager.DomainLayer.Repositories;

namespace TaskManager.DomainLayer.Service.CustomMenu
{
    internal class DeveloperMenu
    {
        private readonly Menu _developerMenu;
        private readonly User _developer;

        public DeveloperMenu(User developer)
        {
            _developer = developer;
            string[] developerMenuOptions = { "Alterar senha", "Criar tarefa", "Sair" };
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
                    //DevTaskRepository.taskList.Add(
                    //    new DevTask(
                    //        techLeaderLogin: "kaio",
                    //        title: "Nova tarefa criada por dev",
                    //        developerLogin: _developer.Login
                    //        )
                    //    );
                    Console.WriteLine("Tarefa teste criada, verificar em Tarefas.");
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
