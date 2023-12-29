using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.ConsoleInteraction;
using TaskManager.DomainLayer.Model;
using TaskManager.DomainLayer.Repositories;

namespace TaskManager.DomainLayer.Service
{
    internal class MenuService
    {
        private readonly Menu _mainMenu;

        public MenuService()
        {
            string[] mainMenuOptions = { "Realizar Login", "Usuários", "Teste", "Sair" };
            _mainMenu = new Menu(mainMenuOptions);
        }

        public int DisplayMainMenu()
        {
            string title = Title.MainMenu();
            return _mainMenu.DisplayMenu(title);
        }

        public void AnalyzeMainMenu(int selectedOption)
        {
            switch (selectedOption)
            {
                case 0:
                    User user = Authentication.Authenticate(UserRepository.All());
                    break;
                case 1:
                    Title.AllUsers();
                    UserRepository.DisplayAll();
                    break;
                case 2:
                    Developer dev = new Developer("Ana Carolina", "ana", "armentanocarolina@gmail.com");
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
