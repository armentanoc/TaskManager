using TaskManager.Infrastructure.Operations;

namespace TaskManager.Infrastructure
{
    public class DatabaseSetup
    {
        public static void Main()
        {
            DatabaseConnection.EstablishConnection();
            WriteMessages();
        }
        private static void WriteMessages()
        {
            Console.WriteLine($"\nsqlite3 {AppDomain.CurrentDomain.BaseDirectory}{DatabaseConnection.DatabasePath}");
            Console.WriteLine("\nPressione qualquer tecla para avançar para a aplicação.");
            Console.ReadKey();
        }
    }
}
