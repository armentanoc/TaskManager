
using System.Data.SQLite;
using TaskManager.DomainLayer.Operations;

namespace TaskManager.DomainLayer.Service.Database.Operations
{
    internal class Tables
    {
        internal static void Create(SQLiteConnection connection, string tableName, string query)
        {
            try
            {
                if (!DatabaseConnection.DoesTableExist(connection, tableName))
                {
                    string createTableQuery = query;
                    DatabaseConnection.ExecuteNonQuery(createTableQuery);
                    Console.WriteLine($"Tabela {tableName} criada com sucesso.");
                }
                else
                {
                    Console.Write($"A tabela {tableName} já existe.");
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Erro ao criar a tabela: {ex.Message}");
            } 
        }
    }
}
