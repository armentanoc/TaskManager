
using System.Data;
using System.Data.SQLite;
using TaskManager.DomainLayer.Repositories;

namespace TaskManager.DomainLayer.Operations
{
    internal class DatabaseConnection
    {
        internal const string DatabasePath = "task_manager_data.db";

        internal static SQLiteConnection CreateConnection()
        {
            var connection = new SQLiteConnection($"Data Source={DatabasePath};Version=3;");
            try
            {
                connection.Open();
                Console.WriteLine($"\nConexão aberta com sucesso");
                return connection;
            }
            catch (SQLiteException ex)
            {
                HandleDatabaseOperationError("criar conexão", ex);
                return null;
            }
        }

        internal static void CloseConnection(SQLiteConnection connection, string operation)
        {
            if (connection != null && connection.State != System.Data.ConnectionState.Closed)
            {
                connection.Close();
                Console.WriteLine($"Conexão fechada ({operation})");
            }
        }

        private static void HandleDatabaseOperationError(string operationType, SQLiteException ex, string? query = null)
        {
            Console.WriteLine($"Operação falhou ({operationType})");
            if (!string.IsNullOrEmpty(query))
            {
                Console.WriteLine($"Consulta: {query}");
            }
            Console.WriteLine($"Erro: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
        }

        public static void EstablishConnection()
        {
            using (var connection = CreateConnection())
            {
                if (connection == null)
                    return;

                InitializeDatabase(connection);
                UserRepository.Initialize();
                DevTaskRepository.Initialize();
            }
        }

        internal static void InitializeDatabase(SQLiteConnection connection)
        {
            string operation = "inicializar banco de dados";

            try
            {
                if (!File.Exists(DatabasePath))
                {
                    try
                    {
                        // Código de inicialização do esquema do banco de dados pode ser adicionado aqui
                        Console.WriteLine($"Arquivo do banco de dados criado com sucesso: {DatabasePath}");
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine($"Erro ao criar arquivo do banco de dados: {ex.Message}");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine($"Arquivo do banco de dados já existe: {DatabasePath}");
                }
                Console.WriteLine("Banco de dados inicializado com sucesso.");
                CloseConnection(connection, "inicializar banco de dados");
            }
            catch (SQLiteException ex)
            {
                HandleDatabaseOperationError(operation, ex);
            }
        }

        public static void ExecuteWithinTransaction(Action<SQLiteConnection> action)
        {
            using (var connection = CreateConnection())
            {
                if (connection == null)
                    return;

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        action.Invoke(connection);
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        CloseConnection(connection, "within transaction");
                    }
                }
            }
        }
        internal static DataTable ExecuteQuery(string query)
        {
            string operation = "fetch data from table";

            try
            {
                using (var connection = CreateConnection())
                {
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        using (var adapter = new SQLiteDataAdapter(command))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            CloseConnection(connection, "query executada com sucesso e salva em uma variável");
                            return dataTable;
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                HandleDatabaseOperationError(operation, ex, query);
                return null;
            }
        }
        internal static void ExecuteNonQuery(string query, object parameters = null)
        {
            string operation = "executar alterações no banco";
            try
            {
                using (var connection = CreateConnection())
                {
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        using (var adapter = new SQLiteDataAdapter(command))
                        {

                            AddParameters(command, parameters);
                            command.ExecuteNonQuery();
                            CloseConnection(connection, "alteração executada com sucesso");
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                HandleDatabaseOperationError(operation, ex, query);
            }
        }
        internal static bool DoesTableExist(SQLiteConnection connection, string tableName)
        {
            const string query = "SELECT name FROM sqlite_master WHERE type='table' AND name=@TableName;";
            var parameters = new Dictionary<string, object> { { "@TableName", tableName } };

            using (var command = new SQLiteCommand(query, connection))
            {
                AddParameters(command, parameters);
                object result = command.ExecuteScalar();
                return result != null && result.ToString() == tableName;
            }
        }
        internal static object ExecuteScalar(SQLiteConnection connection, string query, Dictionary<string, object> parameters = null)
        {
            string operation = "executar escalar";

            try
            {
                using (var command = new SQLiteCommand(query, connection))
                {
                    AddParameters(command, parameters);
                    return command.ExecuteScalar();
                }
            }
            catch (SQLiteException ex)
            {
                HandleDatabaseOperationError(operation, ex, query);
                return null;
            }
        }

        private static void AddParameters(SQLiteCommand command, object parameters)
        {
            if (parameters is Dictionary<string, object> paramDict)
            {
                foreach (var parameter in paramDict)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
            }
        }
    }
}
