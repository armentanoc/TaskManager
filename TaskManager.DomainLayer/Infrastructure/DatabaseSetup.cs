
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Infrastructure.Operations;

namespace TaskManager.DomainLayer.Infrastructure
{
    public class DatabaseSetup
    {
        public static void Execute()
        {
            ConsoleSpinner spin = new();
            ShowFunnyMessage();

            bool doWeHaveTheData;
            doWeHaveTheData = !DatabaseConnection.EstablishConnection();

            DateTime startTime = DateTime.Now;

            while (doWeHaveTheData == null || (DateTime.Now - startTime).TotalSeconds < 3)
            {
                ShowFunLoadingAnimationStars();
            }

            Console.CursorVisible = true;
        }

        private static void ShowFunnyMessage()
        {
            Console.Write("\n              Trabalhando no Banco de Dados...\n\n               _\r\n             | |\r\n             | |===( )   //////\r\n             |_|   |||  | o o|\r\n                    ||| ( c  )                  ____\r\n                     ||| \\= /                  ||   \\_\r\n                      ||||||                   ||     |\r\n                      ||||||                ...||__/|-\"\r\n                      ||||||             __|________|__\r\n                        |||             |______________|\r\n                        |||             || ||      || ||\r\n                        |||             || ||      || ||\r\n------------------------|||-------------||-||------||-||-------\r\n                        |__>            || ||      || || \n\n\n");
            Console.CursorVisible = false;
        }

        public static string WriteDatabasePath()
        {
            string databasePath = $"{AppDomain.CurrentDomain.BaseDirectory}{DatabaseConnection.DatabasePath}";
            return Message.ShowDatabasePath(databasePath);
        }
        static void ShowFunLoadingAnimationStars()
        {
            char bouncingChar = '*';
            int bounces = 20;
            for (int i = 0; i < bounces; i++)
            {
                Console.Write($"\r{GetSpaces(i)}{bouncingChar}{GetSpaces(bounces - i - 1)}");
                Thread.Sleep(150);
            }
        }
        static string GetSpaces(int count)
        {
            return new string(' ', count);
        }
    }
}

