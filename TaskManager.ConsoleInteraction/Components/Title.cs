





namespace TaskManager.ConsoleInteraction.Components
{
    public class Title
    {
        //https://www.asciiart.eu/text-to-ascii-art
        public static void AllUsers()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(" _   _             __       _           \r\n| | | |___ _   _  /_/_ _ __(_) ___  ___ \r\n| | | / __| | | |/ _` | '__| |/ _ \\/ __|\r\n| |_| \\__ \\ |_| | (_| | |  | | (_) \\__ \\\r\n \\___/|___/\\__,_|\\__,_|_|  |_|\\___/|___/");
            Console.ResetColor();
        }

        public static string MainMenu()
        {
            return "  _____         _      __  __                                   \r\n|_   _|_ _ ___| | __ |  \\/  | __ _ _ __   __ _  __ _  ___ _ __ \r\n  | |/ _` / __| |/ / | |\\/| |/ _` | '_ \\ / _` |/ _` |/ _ \\ '__|\r\n  | | (_| \\__ \\   <  | |  | | (_| | | | | (_| | (_| |  __/ |   \r\n  |_|\\__,_|___/_|\\_\\ |_|  |_|\\__,_|_| |_|\\__,_|\\__, |\\___|_|   \r\n                                               |___/           ";
        }

        public static string GreetingDev()
        {
            return " _   _      _ _          ____             _ \r\n| | | | ___| | | ___    |  _ \\  _____   _| |\r\n| |_| |/ _ \\ | |/ _ \\   | | | |/ _ \\ \\ / / |\r\n|  _  |  __/ | | (_) |  | |_| |  __/\\ V /|_|\r\n|_| |_|\\___|_|_|\\___( ) |____/ \\___| \\_/ (_)\r\n                    |/                      ";
        }

        public static string GreetingTechLead()
        {
            return "  _   _      _ _          _____         _       _                   _ _ \r\n| | | | ___| | | ___    |_   _|__  ___| |__   | |    ___  __ _  __| | |\r\n| |_| |/ _ \\ | |/ _ \\     | |/ _ \\/ __| '_ \\  | |   / _ \\/ _` |/ _` | |\r\n|  _  |  __/ | | (_) |    | |  __/ (__| | | | | |__|  __/ (_| | (_| |_|\r\n|_| |_|\\___|_|_|\\___( )   |_|\\___|\\___|_| |_| |_____\\___|\\__,_|\\__,_(_)\r\n                    |/                                                 ";
        }

        public static void Login()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" _                _       \r\n| |    ___   __ _(_)_ __  \r\n| |   / _ \\ / _` | | '_ \\ \r\n| |__| (_) | (_| | | | | |\r\n|_____\\___/ \\__, |_|_| |_|\r\n            |___/         ");
            Console.ResetColor();
        }

        public static void Error()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" _____                _ \r\n| ____|_ __ _ __ ___ | |\r\n|  _| | '__| '__/ _ \\| |\r\n| |___| |  | | | (_) |_|\r\n|_____|_|  |_|  \\___/(_)");
            Console.ResetColor();
        }

        public static void ChangePassword()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(" _   _                   ____             _           \r\n| \\ | | _____   ____ _  / ___|  ___ _ __ | |__   __ _ \r\n|  \\| |/ _ \\ \\ / / _` | \\___ \\ / _ \\ '_ \\| '_ \\ / _` |\r\n| |\\  | (_) \\ V / (_| |  ___) |  __/ | | | | | | (_| |\r\n|_| \\_|\\___/ \\_/ \\__,_| |____/ \\___|_| |_|_| |_|\\__,_|");
            Console.ResetColor();
        }

        internal static void Returning()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("__     __    _ _                  _            \r\n\\ \\   / /__ | | |_ __ _ _ __   __| | ___       \r\n \\ \\ / / _ \\| | __/ _` | '_ \\ / _` |/ _ \\      \r\n  \\ V / (_) | | || (_| | | | | (_| | (_) | _ _ \r\n   \\_/ \\___/|_|\\__\\__,_|_| |_|\\__,_|\\___(_|_|_)");
            Console.ResetColor();
        }

        internal static void AskForJSONPath()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("    _       _     _         _             _ ____   ___  _   _ \r\n   / \\   __| | __| | __   _(_) __ _      | / ___| / _ \\| \\ | |\r\n  / _ \\ / _` |/ _` | \\ \\ / / |/ _` |  _  | \\___ \\| | | |  \\| |\r\n / ___ \\ (_| | (_| |  \\ V /| | (_| | | |_| |___) | |_| | |\\  |\r\n/_/   \\_\\__,_|\\__,_|   \\_/ |_|\\__,_|  \\___/|____/ \\___/|_| \\_|");
            Console.ResetColor();
        }

        public static void EnvironmentExit()
        {
            Console.WriteLine(" ____                                             \r\n|  _ \\ _ __ ___   __ _ _ __ __ _ _ __ ___   __ _  \r\n| |_) | '__/ _ \\ / _` | '__/ _` | '_ ` _ \\ / _` | \r\n|  __/| | | (_) | (_| | | | (_| | | | | | | (_| | \r\n|_|   |_|  \\___/ \\__, |_|  \\__,_|_| |_| |_|\\__,_| \r\n  ___ _ __   ___ |___/ __ _ __ __ _  __| | ___ | |\r\n / _ \\ '_ \\ / __/ _ \\ '__| '__/ _` |/ _` |/ _ \\| |\r\n|  __/ | | | (_|  __/ |  | | | (_| | (_| | (_) |_|\r\n \\___|_| |_|\\___\\___|_|  |_|  \\__,_|\\__,_|\\___/(_)");
        }

        public static void AllTasks()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(" _____         _        \r\n|_   _|_ _ ___| | _____ \r\n  | |/ _` / __| |/ / __|\r\n  | | (_| \\__ \\   <\\__ \\\r\n  |_|\\__,_|___/_|\\_\\___/");
            Console.ResetColor();
        }
    }
}