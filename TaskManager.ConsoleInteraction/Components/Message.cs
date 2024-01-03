
using TaskManager.Logger;

namespace TaskManager.ConsoleInteraction.Components
{
    public class Message
    {
        static LogWriter _logWriter;

        public static void PasswordChangedSuccessfully(string prompt) {
            LogAndConsoleWrite(prompt);
        }
        public static string? AskForJSONPath()
        {
            Title.AskForJSONPath();

            Console.WriteLine(
                "\nObs.: O arquivo JSON deve estar localizado em TaskManager.DomainLayer.Files. " +
                "\n\nDigite o caminho relativo do arquivo JSON (ex.: devs.json):");

            LogWrite("O path do arquivo JSON foi solicitado");
            return Console.ReadLine();
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
        public static string ShowDatabasePath(string databasePath)
        {
            string prompt = $"Database: sqlite3 {databasePath}";
            LogWrite(prompt);
            return prompt;
        }
        public static void InitializeDefaultDevTasks()
        {
            LogWrite("\nInicializando tarefas padrão em DevTasks");
        }
        public static void InitializeDefaultDevTaskRelationships()
        {
            LogWrite("\nInicializando relacionamentos padrão em DevTaskRelationships");
        }
        public static void InitializeDatabase()
        {
            _logWriter.LogWrite("Inicializando Database");
        }
        public static void NewUsersInUserList()
        {
            Console.WriteLine("\nDigite qualquer tecla para visualizar a lista de usuários...");
            Console.ReadKey();
        }

        // error messages
        public static void Error(string prompt)
        {
            Title.Error();
            LogAndConsoleWrite(prompt);
            PressAnyKeyToReturn();
        }
        public static void AuthenticationFailed()
        {
            Title.Error();
            LogAndConsoleWrite("Falha na autenticação. Tente novamente.");
        }
        public static void CatchException(Exception ex)
        {
            Error(ex.Message);
        }
        public static void IncorrectPassword()
        {
            Title.Error();
            LogAndConsoleWrite("Senha incorreta. Tente novamente.");
        }
        public static void IncorrectUser()
        {
            Title.Error();
            LogAndConsoleWrite("Usuário não existe. Tente novamente.");
        }
        public static void NoUserWithThatLogin()
        {
            Title.Error();
            LogAndConsoleWrite("Não há funcionário com esse login. Tente novamente.");
        }
        public static void PasswordIsNullOrWhitespace()
        {
            Title.Error();
            LogAndConsoleWrite("A senha não pode ser nula ou composta apenas de espaços em branco. Tente novamente.");
        }

        // log write
        public static void LogWrite(string prompt)
        {
            _logWriter = new LogWriter(prompt);
        }
        public static void LogAndConsoleWrite(string prompt)
        {
            LogWrite(prompt);
            Console.WriteLine($"\n{prompt}");
        }
    }
}
