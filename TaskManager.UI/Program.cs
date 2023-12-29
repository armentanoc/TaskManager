﻿using TaskManager.ConsoleInteraction;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Service.MenuAnalyzer;

namespace TaskManager.UI
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
                    MainMenu.Analyze(userSelection);
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
