using TaskManager.DomainLayer.Operations;
using TaskManager.ConsoleInteraction.Components;

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
            string databasePath = $"{AppDomain.CurrentDomain.BaseDirectory}{DatabaseConnection.DatabasePath}";
            Message.ShowDatabasePath(databasePath);
            Message.Divider();
            Message.PressAnyKeyToGoFoward();
        }
    }
}
