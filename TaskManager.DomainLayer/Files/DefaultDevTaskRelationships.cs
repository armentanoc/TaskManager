using System.Data.SQLite;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Infrastructure.Operations;
using TaskManager.DomainLayer.Infrastructure.Repositories;
using TaskManager.DomainLayer.Model.Tasks;

namespace TaskManager.DomainLayer.Files
{
    internal class DefaultDevTaskRelationships
    {
        internal static void InitializeDefaultRelationships()
        {
            SQLiteConnection? defaultConnection = null;

            try
            {
                Message.InitializeDefaultDevTaskRelationships();

                defaultConnection = DatabaseConnection.CreateConnection($"inserir relacionamentos padrão em {DevTaskRelationshipRepo.TableName}");
                var relationships = DevTaskRelationshipRepo.GetDevTaskRelationshipsList();

                if (relationships.Count == 0)
                {
                    InsertDefaultTasksRelationships(defaultConnection);
                }
            }
            finally
            {
                DatabaseConnection.CloseConnection(defaultConnection, "inserir tarefas padrão em DevTasks");
            }
        }

        private static void InsertDefaultTasksRelationships(SQLiteConnection connection)
        {
            var defaultRelationship = new DevTaskRelationship
            (
                parentOrFirst : "1",
                childOrSecond : "2",
                relationshipType : RelationshipTypeEnum.ParentChild
            );

            DevTaskRelationshipRepo.InsertDevTaskRelationshipIfNotExists(connection, defaultRelationship);
        }
    }
}
