
using System.Data.SQLite;
using TaskManager.UI;

namespace TaskManager.DomainLayer.Infrastructure.Operations
{
    internal class Tables
    {
        static LogWriter _logWriter;
        internal static void Create(SQLiteConnection connection, string tableName, string query)
        {
            try
            {
                if (!DatabaseConnection.DoesTableExist(connection, tableName))
                {
                    string createTableQuery = query;
                    DatabaseConnection.ExecuteNonQuery(createTableQuery);
                    _logWriter = new LogWriter($"Tabela {tableName} criada com sucesso.");
                }
                else
                {

                    _logWriter = new LogWriter($"A tabela {tableName} já existe.");
                }
            }
            catch (SQLiteException ex)
            {
                _logWriter = new LogWriter($"Erro ao criar a tabela: {ex.Message}");
            }
        }
    }
}
