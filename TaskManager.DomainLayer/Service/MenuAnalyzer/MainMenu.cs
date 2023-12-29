﻿using System.Net.Security;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Model;
using TaskManager.DomainLayer.Repositories;

namespace TaskManager.DomainLayer.Service.MenuAnalyzer
{
    public class MainMenu
    {
        public static void Analyze(int userSelection)
        {
            switch (userSelection)
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
