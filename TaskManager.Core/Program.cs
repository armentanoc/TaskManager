using TaskManager.ConsoleInteraction;

namespace TaskManager.Core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] mainMenu = { "Um", "Dois", "Três", "Sair" };
            Menu options = new Menu(mainMenu);

            while (true)
            {
                string title = Title.MainMenu();
                int userSelection = options.DisplayMenu(title);
                MenuAnalyzer.MainMenu(userSelection);
                Menu.PressAnyKeyToReturn();
            }
        }
    }
}
