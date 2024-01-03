
using System.Data.SQLite;

namespace TaskManager.DomainLayer.Infrastructure.Operations.UserRepositoryOperations
{
    internal class Create
    {
        internal static void Table(SQLiteConnection connection, string tableName)
        {
            string createUsersQuery = $@"
                    CREATE TABLE IF NOT EXISTS {tableName} (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Login TEXT NOT NULL,
                        Password TEXT NOT NULL,
                        Email TEXT,
                        JobType TEXT NOT NULL
                    );";

            Tables.Create(connection, tableName, createUsersQuery);
        }
    }
}
