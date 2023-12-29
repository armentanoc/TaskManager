
namespace TaskManager.ConsoleInteraction.Components
{
    public class Title
    {
        //https://www.asciiart.eu/text-to-ascii-art
        public static void AllUsers()
        {
            Console.Clear();
            Console.WriteLine(" _   _             __       _           \r\n| | | |___ _   _  /_/_ _ __(_) ___  ___ \r\n| | | / __| | | |/ _` | '__| |/ _ \\/ __|\r\n| |_| \\__ \\ |_| | (_| | |  | | (_) \\__ \\\r\n \\___/|___/\\__,_|\\__,_|_|  |_|\\___/|___/");
        }

        public static string MainMenu()
        {
            return "  _____         _      __  __                                   \r\n|_   _|_ _ ___| | __ |  \\/  | __ _ _ __   __ _  __ _  ___ _ __ \r\n  | |/ _` / __| |/ / | |\\/| |/ _` | '_ \\ / _` |/ _` |/ _ \\ '__|\r\n  | | (_| \\__ \\   <  | |  | | (_| | | | | (_| | (_| |  __/ |   \r\n  |_|\\__,_|___/_|\\_\\ |_|  |_|\\__,_|_| |_|\\__,_|\\__, |\\___|_|   \r\n                                               |___/           ";
        }

        public static void GreetingDev()
        {
            Console.Clear();
            Console.WriteLine(" _   _      _ _          ____             _ \r\n| | | | ___| | | ___    |  _ \\  _____   _| |\r\n| |_| |/ _ \\ | |/ _ \\   | | | |/ _ \\ \\ / / |\r\n|  _  |  __/ | | (_) |  | |_| |  __/\\ V /|_|\r\n|_| |_|\\___|_|_|\\___( ) |____/ \\___| \\_/ (_)\r\n                    |/                      ");
        }

        public static void GreetingTechLead()
        {
            Console.Clear();
            Console.WriteLine("  _   _      _ _          _____         _       _                   _ _ \r\n| | | | ___| | | ___    |_   _|__  ___| |__   | |    ___  __ _  __| | |\r\n| |_| |/ _ \\ | |/ _ \\     | |/ _ \\/ __| '_ \\  | |   / _ \\/ _` |/ _` | |\r\n|  _  |  __/ | | (_) |    | |  __/ (__| | | | | |__|  __/ (_| | (_| |_|\r\n|_| |_|\\___|_|_|\\___( )   |_|\\___|\\___|_| |_| |_____\\___|\\__,_|\\__,_(_)\r\n                    |/                                                 ");
        }

        public static void Login()
        {
            Console.Clear();
            Console.WriteLine(" _                _       \r\n| |    ___   __ _(_)_ __  \r\n| |   / _ \\ / _` | | '_ \\ \r\n| |__| (_) | (_| | | | | |\r\n|_____\\___/ \\__, |_|_| |_|\r\n            |___/         ");
        }

        public static void Error()
        {
            Console.Clear();
            Console.WriteLine(" _____                _ \r\n| ____|_ __ _ __ ___ | |\r\n|  _| | '__| '__/ _ \\| |\r\n| |___| |  | | | (_) |_|\r\n|_____|_|  |_|  \\___/(_)");
        }
    }
}