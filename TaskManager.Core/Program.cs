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
                AnalyzeUserChoice(userSelection);
                Menu.PressAnyKeyToReturn();
            }
        }
        static void AnalyzeUserChoice(int userSelection)
        {
            switch (userSelection)
            {
                case 0:
                    Console.WriteLine("Um");
                    break;
                case 1:
                    Console.WriteLine("Dois");
                    break;
                case 2:
                    Console.WriteLine("Três");
                    break;
                case 3:
                    Console.WriteLine("Programa encerrado");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Opção inválida");
                    break;
            }
        }
    }
}
