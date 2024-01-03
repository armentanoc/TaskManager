
using System.Data.SQLite;

namespace TaskManager.DomainLayer.Infrastructure.Operations.DevTaskRepositoryOperations
{
    internal class Create
    {
        internal static void Table(SQLiteConnection connection, string tableName)
        {
            string createDevTasksQuery = $@"

                    CREATE TABLE {tableName} (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Description TEXT,
                    TechLeaderLogin TEXT NOT NULL,
                    DeveloperLogin TEXT,
                    Status TEXT NOT NULL,
                    RequiresApprovalToComplete TEXT NOT NULL,
                    Deadline DATETIME,
                    CompletionDateTime DATETIME,
                    FOREIGN KEY (TechLeaderLogin) REFERENCES Users(Login),
                    FOREIGN KEY (DeveloperLogin) REFERENCES Users(Login)
                    )";

            Tables.Create(connection, tableName, createDevTasksQuery);
        }
    }
}
