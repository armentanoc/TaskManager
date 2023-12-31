﻿
using TaskManager.ConsoleInteraction.Components;
using TaskManager.ConsoleInteraction;
using TaskManager.DomainLayer.Service.Login;
using TaskManager.DomainLayer.Model.People;
using TaskManager.DomainLayer.Infrastructure.Repositories;
using System.Reflection;
using TaskManager.DomainLayer.Infrastructure;
using System.Text;

namespace TaskManager.DomainLayer.Service.CustomMenu
{
    internal class MainMenu
    {
        private readonly Menu _mainMenu;

        public MainMenu()
        {
            string[] mainMenuOptions = { "Realizar Login", "Usuários", "Tarefas", "Relacionamentos", "Sair" };
            _mainMenu = new Menu(mainMenuOptions);
        }

        public int DisplayMainMenu()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Title.MainMenu());
            sb.AppendLine($"\nLog: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            sb.AppendLine(DatabaseSetup.WriteDatabasePath());
            sb.Append("Obs.: senha padrão é 1234");
            return _mainMenu.DisplayMenu(sb.ToString());
        }

        public void AnalyzeMainMenu(int selectedOption)
        {
            switch (selectedOption)
            {
                case 0:
                    User user = Authentication.Authenticate();
                    break;
                case 1:
                    UserRepo.DisplayAll();
                    break;
                case 2:
                    DevTaskRepo.DisplayAll();
                    break;
                case 3:
                    DevTaskRelationshipRepo.DisplayAll();
                    break;
                case 4:
                    Title.EnvironmentExit();
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Opção inválida");
                    break;
            }
        }
    }
}
