
using System.Data;
using System.Data.SQLite;
using TaskManager.DomainLayer.Infrastructure.Repositories;
using TaskManager.UI;

namespace TaskManager.DomainLayer.Infrastructure.Operations
{
    internal class DatabaseConnection
    {
        internal const string DatabasePath = "task_manager_data.db";
        static LogWriter _logWriter;

        internal static SQLiteConnection? CreateConnection(string context)
        {
            var connection = new SQLiteConnection($"Data Source={DatabasePath};Version=3;");
            try
            {
                connection.Open();
                _logWriter = new LogWriter($"\nConexão aberta com sucesso ({context})");
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
            if (connection != null && connection.State != ConnectionState.Closed)
            {
                connection.Close();
                _logWriter = new LogWriter($"Conexão fechada ({operation})");
            }
        }
        private static void HandleDatabaseOperationError(string operationType, SQLiteException ex, string? query = null)
        {
            _logWriter = new LogWriter($"Operação falhou ({operationType})");
            if (!string.IsNullOrEmpty(query))
            {
                _logWriter = new LogWriter($"Consulta: {query}");
            }
            _logWriter = new LogWriter($"Erro: {ex.Message}");
            _logWriter = new LogWriter($"StackTrace: {ex.StackTrace}");
        }
        public static bool EstablishConnection()
        {

            using (var connection = CreateConnection("inicializar banco de dados"))
            {
                if (connection == null)
                    return false;
                InitializeDatabase(connection);

                LogWriter logWriter;

                logWriter = new LogWriter("Inicialização de Users começou");
                UserRepository.Initialize();

                logWriter = new LogWriter("Inicialização de DevTasks começou");
                DevTaskRepository.Initialize();

                logWriter = new LogWriter("Inicialização de DevTasksRelationships começou");
                DevTaskRelationshipRepository.Initialize();

                return true;
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
                        _logWriter = new LogWriter($"Arquivo do banco de dados criado com sucesso: {DatabasePath}");
                    }
                    catch (IOException ex)
                    {
                        _logWriter = new LogWriter($"Erro ao criar arquivo do banco de dados: {ex.Message}");
                        return;
                    }
                }
                else
                {
                    _logWriter = new LogWriter($"Arquivo do banco de dados já existe: {DatabasePath}");
                }
                _logWriter = new LogWriter("Banco de dados inicializado com sucesso.");
                CloseConnection(connection, "inicializar banco de dados");
            }
            catch (SQLiteException ex)
            {
                HandleDatabaseOperationError(operation, ex);
            }
        }
        public static void ExecuteWithinTransaction(Action<SQLiteConnection> action)
        {
            try
            {
                using (var connection = CreateConnection("within transaction"))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            action(connection);
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro executando within transaction: {ex.Message}");
            }
        }
        internal static DataTable? ExecuteQuery(string query)
        {
            string operation = "fetch data from table";

            try
            {
                using (var connection = CreateConnection("executar query"))
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
                using (var connection = CreateConnection("executar non-query"))
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
            _logWriter = new LogWriter($"Verificando se a tabela {tableName} já existe...");
            const string query = "SELECT name FROM sqlite_master WHERE type='table' AND name=@TableName;";
            var parameters = new Dictionary<string, object> { { "@TableName", tableName } };

            using (var command = new SQLiteCommand(query, connection))
            {
                AddParameters(command, parameters);
                object result = command.ExecuteScalar();
                return result != null && result.ToString() == tableName;
            }
        }
        internal static object? ExecuteScalar(SQLiteConnection connection, string query, Dictionary<string, object> parameters = null)
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
