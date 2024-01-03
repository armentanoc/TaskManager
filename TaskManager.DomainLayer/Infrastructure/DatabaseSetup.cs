using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Infrastructure.Operations;

namespace TaskManager.DomainLayer.Infrastructure
{
    public class DatabaseSetup
    {
        public static void Execute()
        {
            DatabaseConnection.EstablishConnection();
        }
        public static string WriteDatabasePath()
        {
            string databasePath = $"{AppDomain.CurrentDomain.BaseDirectory}{DatabaseConnection.DatabasePath}";
            return Message.ShowDatabasePath(databasePath);
        }
    }
}
