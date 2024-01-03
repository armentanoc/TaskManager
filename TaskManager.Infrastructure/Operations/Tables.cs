
using System.Data.SQLite;

namespace TaskManager.Infrastructure.Operations
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
                    DatabaseConnection.ExecuteNonQuery(connection, createTableQuery);
                    Console.WriteLine($"Tabela {tableName} criada com sucesso.");
                }
                else
                {
                    Console.WriteLine($"A tabela {tableName} já existe.");
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Erro ao criar a tabela: {ex.Message}");
            }
        }
    }
}
