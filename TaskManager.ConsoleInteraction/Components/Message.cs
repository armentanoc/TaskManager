
namespace TaskManager.ConsoleInteraction.Components
{
    public class Message
    {
        public static string? AskForJSONPath()
        {
            Title.AskForJSONPath();
            Console.WriteLine("\nObs.: O arquivo JSON deve estar localizado em TaskManager.DomainLayer.Files. " +
                "\n\nDigite o caminho relativo do arquivo JSON (ex.: devs.json):");
            return Console.ReadLine();
        }

        public static void AuthenticationFailed()
        {

            Title.Error();
            Console.WriteLine("\nFalha na autenticação. Tente novamente.");
        }

        public static void CatchException(Exception ex)
        {
            Console.WriteLine($"\n{ex.Message}");
        }
        public static void IncorrectPassword()
        {
            Title.Error();
            Console.WriteLine("\nSenha incorreta. Tente novamente.");
            PressAnyKeyToReturn();
        }

        public static void IncorrectUser()
        {
            Title.Error();
            Console.WriteLine("\nUsuário não existe. Tente novamente.");
            PressAnyKeyToReturn();
        }

        public static void NewUsersInUserList()
        {
            Console.WriteLine("\nDigite qualquer tecla para visualizar a lista de usuários...");
            Console.ReadKey();
        }

        public static void NoUserWithThatLogin()
        {
            Title.Error();
            Console.WriteLine("Não há funcionário com esse login. Tente novamente.");
            PressAnyKeyToReturn();
        }

        public static void PasswordIsNullOrWhitespace()
        {
            Title.Error();
            Console.WriteLine("\nA senha não pode ser nula ou composta apenas de espaços em branco. Tente novamente.");
            PressAnyKeyToReturn();
        }

        public static void PasswordChanged()
        {

            Console.WriteLine("\nSenha alterada com sucesso.");
            PressAnyKeyToReturn();

        }

        public static void PressAnyKeyToContinue()
        {
            Console.WriteLine("\nPressione qualquer tecla para continuar.");
            Console.ReadKey();
        }
        public static void PressAnyKeyToReturn()
        {
            Console.WriteLine("\nPressione qualquer tecla para retornar.");
        }

        public static void Returning()
        {
            Title.Returning();
        }

        public static void PressAnyKeyToGoFoward()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nPressione qualquer tecla para avançar para a aplicação.");
            Console.ResetColor();
            Console.ReadKey();
        }

        public static void Divider()
        {
            Console.WriteLine("\n-------------------------------------------------");
        }

        public static void ShowDatabasePath(string databasePath)
        {
            Divider();
            Console.WriteLine("\nPARA DEBUGGAR LOCALMENTE NO TERMINAL");
            Console.WriteLine($"\nsqlite3 {databasePath}");
        }

        public static void InitializeDefaultDevTasks()
        {
            Divider();
            Console.WriteLine("\nINICIALIZAR TAREFAS PADRÃO EM DEVTASKS");
        }
    }
}
