using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.ConsoleInteraction;
using TaskManager.Core.People;
using TaskManager.Core.Repository;

namespace TaskManager.Core
{
    internal class MenuAnalyzer
    {
        internal static void MainMenu(int userSelection)
        {
            switch (userSelection)
            {
                case 0:
                    Console.WriteLine("Um");
                    break;
                case 1:
                    Title.AllUsers();
                    UserRepository.DisplayAll();
                    break;
                case 2:
                    Developer dev = new Developer("Ana Carolina", "ana", "123");
                    Console.WriteLine(dev.ToString());
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
