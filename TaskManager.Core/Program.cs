using TaskManager.ConsoleInteraction;
using TaskManager.Core.People;

namespace TaskManager.Core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] mainMenu = { "Um", "Usuários", "Teste", "Sair" };
            Menu options = new Menu(mainMenu);

            try
            {
                while (true)
                {
                    string title = Title.MainMenu();
                    int userSelection = options.DisplayMenu(title);
                    MenuAnalyzer.MainMenu(userSelection);
                    Menu.PressAnyKeyToReturn();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
