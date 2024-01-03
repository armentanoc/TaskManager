
using System.Data.SQLite;

namespace TaskManager.DomainLayer.Infrastructure.Operations.DevTaskRelationshipsRepositoryOperations
{
    internal class Create
    {
        internal static void Table(SQLiteConnection connection, string tableName)
        {
            string createDevTasksQuery = $@"

                    CREATE TABLE {tableName} (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ParentOrFirst_DevTask_Id TEXT NOT NULL,
                    ChildOrSecond_DevTask_Id TEXT NOT NULL,
                    RelationshipType TEXT NOT NULL,
                    FOREIGN KEY (ParentOrFirst_DevTask_Id) REFERENCES DevTasks(Id),
                    FOREIGN KEY (ChildOrSecond_DevTask_Id) REFERENCES DevTasks(Id)
                    )";

            Tables.Create(connection, tableName, createDevTasksQuery);
        }
    }
}
