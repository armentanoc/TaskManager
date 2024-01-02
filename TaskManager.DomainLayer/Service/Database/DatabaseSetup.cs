using TaskManager.DomainLayer.Operations;
using TaskManager.DomainLayer.Service.Database.Operations;

namespace TaskManager.DomainLayer.Service.Database
{
    public class DatabaseSetup
    {
        public static void Execute()
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
