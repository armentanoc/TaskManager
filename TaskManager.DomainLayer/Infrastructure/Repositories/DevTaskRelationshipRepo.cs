
using System.Data;
using System.Data.SQLite;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Infrastructure.Operations;
using TaskManager.DomainLayer.Infrastructure.Operations.DevTaskRelationshipsRepositoryOperations;
using TaskManager.DomainLayer.Model.Tasks;

namespace TaskManager.DomainLayer.Infrastructure.Repositories
{
    internal static class DevTaskRelationshipRepo
    {
        internal const string TableName = "DevTaskRelationships";

        // initialize methods
        internal static void Initialize()
        {
            using (var connection = DatabaseConnection.CreateConnection($"inicializar tabela {TableName}"))
            {
                Create.Table(connection, TableName);
                DatabaseConnection.CloseConnection(connection, $"inicializar tabela {TableName}");
            }

            Files.DefaultDevTaskRelationships.InitializeDefaultRelationships();
        }
        internal static void InitializeNewDevTaskRelationship(DevTaskRelationship newRelation)
        {
            using (var defaultConnection = DatabaseConnection.CreateConnection($"Inserting new task into {TableName}"))
            {
                try
                {
                    Message.InitializeDefaultDevTaskRelationships();
                    var tasks = GetDevTaskRelationshipsList();
                    InsertDevTaskRelationshipIfNotExists(defaultConnection, newRelation);
                }
                catch (Exception ex)
                {
                    Message.CatchException(ex);
                }
                finally
                {
                    DatabaseConnection.CloseConnection(defaultConnection, "Inserting new task into DevTasks");
                }
            }
        }

        // insert methods
        public static void AddDevTaskToDatabase(DevTaskRelationship relationship)
        {
            DatabaseConnection.ExecuteWithinTransaction((transactionConnection) =>
            {
                try
                {
                    InsertDevTaskRelationshipIfNotExists(transactionConnection, relationship);
                }
                catch (Exception ex)
                {
                    Message.Error($"Erro executando transação: {ex.Message}");
                    throw;
                }
            });
        }
        internal static void InsertDevTaskRelationshipIfNotExists(SQLiteConnection connection, DevTaskRelationship relationship)
        {
            try
            {
                if (!DoesRelationshipExist(connection, relationship))
                {
                    InsertRelationship(relationship);
                }
                else
                {
                    throw new Exception($"DevTaskRelationship entre DevTask de Id {relationship.ParentOrFirstTaskId} e DevTask de Id {relationship.ChildOrSecondTaskId} já existe.");
                }
            }
            catch (SQLiteException ex)
            {
                Message.Error($"Erro ao inserir as tarefas DevTasks na tabela: {ex.Message}");
            }
            catch (Exception ex)
            {
                Message.Error($"Erro ao inserir as tarefas DevTasks na tabela: {ex.Message}");
            }
        }
        private static void InsertRelationship(DevTaskRelationship relation)
        {
            try
            {
                string insertRelationshipQuery = $@"
                    INSERT INTO {TableName} (Id, ParentOrFirst_DevTask_Id, ChildOrSecond_DevTask_Id, RelationshipType)
                    VALUES (@Id, @ParentOrFirst, @ChildOrSecond, @RelationshipType);";

                var parameters = new Dictionary<string, object>
                {
                    { "@Id", relation.Id },
                    { "@ParentOrFirst", relation.ParentOrFirstTaskId },
                    { "@ChildOrSecond", relation.ChildOrSecondTaskId },
                    { "@RelationshipType", relation.RelationshipType.ToString() },
                };

                DatabaseConnection.ExecuteNonQuery(insertRelationshipQuery, parameters);
                Message.LogAndConsoleWrite($"Relação '{relation.RelationshipType}' " +
                    $"inserida com sucesso na tabela entre pai/primeiro ({relation.ParentOrFirstTaskId}) " +
                    $"e filho/segundo ({relation.ChildOrSecondTaskId}).");
                Console.WriteLine("\nPressione qualquer tecla para continuar\n");
            }
            catch (SQLiteException ex)
            {
                Message.Error($"Erro ao inserir DevTaskRelationship na tabela: {ex.Message}");
            }
        }

        // validation methods
        private static bool DoesRelationshipExist(SQLiteConnection connection, DevTaskRelationship relationship)
        {
            string query = $"SELECT COUNT(*) FROM {TableName} " +
                           $"WHERE " +
                           $"(ParentOrFirst_DevTask_Id = @ParentOrFirst AND ChildOrSecond_DevTask_Id = @ChildOrSecond) " +
                           $"OR " +
                           $"(ParentOrFirst_DevTask_Id = @ChildOrSecond AND ChildOrSecond_DevTask_Id = @ParentOrFirst);";

            var parameters = new Dictionary<string, object?>
    {
        { "@ParentOrFirst", relationship.ParentOrFirstTaskId },
        { "@ChildOrSecond", relationship.ChildOrSecondTaskId }
    };

            int count = Convert.ToInt32(DatabaseConnection.ExecuteScalar(connection, query, parameters));

            return count > 0;
        }


        // read and display methods
        internal static List<DevTaskRelationship> GetDevTaskRelationshipsList()
        {
            try
            {
                string query = $"SELECT * FROM {TableName};";

                var dataTable = DatabaseConnection.ExecuteQuery(query);
                var relationships = new List<DevTaskRelationship>();

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        var relationship = new DevTaskRelationship(
                            id: row["Id"].ToString(),
                            parentOrFirst: row["ParentOrFirst_DevTask_Id"].ToString(),
                            childOrSecond: row["ChildOrSecond_DevTask_Id"].ToString(),
                            relationshipType: Enum.Parse<RelationshipTypeEnum>(row["RelationshipType"].ToString())
                        );

                        relationships.Add(relationship);
                    }
                }

                return relationships;
            }
            catch (Exception ex)
            {
                Message.Error($"Erro obtendo lista de {TableName}: {ex.Message}");
                return new List<DevTaskRelationship>();
            }
        }
        public static void DisplayAll()
        {
            List<DevTaskRelationship> relationships = GetDevTaskRelationshipsList();
            PrintRelationships(relationships);
        }
        private static void PrintRelationships(List<DevTaskRelationship> relationships)
        {
            Title.AllRelationships();
            foreach (DevTaskRelationship relation in relationships)
            {
                Console.WriteLine(relation.ToString());
            }
        }
    }
}
