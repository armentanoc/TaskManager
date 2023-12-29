
using TaskManager.ConsoleInteraction;
using TaskManager.ConsoleInteraction.Components;

namespace TaskManager.DomainLayer.Model
{
    internal class TechLeader : User
    {
        public TechLeader(string newName, string newLogin, string? newEmail = null) : base(newName, newLogin, newEmail)
        {
            SetJob(JobEnum.TechLeader);
        }

        public override void Greeting()
        {
            MainMenu();
        }

        public void MainMenu()
        {
            string[] menuTechLead = { "Alterar senha", "Sair" };
            Menu options = new Menu(menuTechLead);

            while (true)
            {
                Console.Clear();
                int selectedOption = options.DisplayMenu(Title.GreetingTechLead());
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
                    base.TryChangingPassword();
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
