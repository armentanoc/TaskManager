using TaskManager.ConsoleInteraction;
using TaskManager.ConsoleInteraction.Components;

namespace TaskManager.DomainLayer.Model
{
    internal class Developer : User
    {
        public Developer(string newName, string newLogin, string? newEmail = null) : base(newName, newLogin, newEmail)
        {
            SetJob(JobEnum.Developer);
        }

        public override void Greeting()
        {
            MainMenu();
        }

        public void MainMenu()
        {
            string[] menuDev = { "Alterar senha", "Sair" };
            Menu options = new Menu(menuDev);

            while (true)
            {
                Console.Clear();
                int selectedOption = options.DisplayMenu(Title.GreetingDev());
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
