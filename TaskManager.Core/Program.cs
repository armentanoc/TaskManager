using TaskManager.ConsoleInteraction;

namespace TaskManager.Core
{
    internal class Program : IConsole
    {
        static void Main(string[] args)
        {
            string[] mainMenu = { "Um", "Dois", "Três", "Sair" };
            Menu options = new Menu(mainMenu);

            while (true)
            {
                int userSelection = options.ShowMenu(Title.MainMenu());
                AnalyzeUserChoice(userSelection);
            }
        }
        static void AnalyzeUserChoice(int userSelection)
        {
            switch (userSelection)
            {
                case 0:
                    Console.WriteLine("Um");
                    IConsole.WaitThreeSeconds();
                    break;
                case 1:
                    Console.WriteLine("Dois");
                    IConsole.WaitThreeSeconds();
                    break;
                case 2:
                    Console.WriteLine("Três");
                    IConsole.WaitThreeSeconds();
                    break;
                case 3:
                    Console.WriteLine("Programa encerrado");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Opção inválida");
                    IConsole.WaitThreeSeconds();
                    break;
            }
        }
    }
}
