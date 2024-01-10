
using System.Data.SQLite;
using TaskManager.DomainLayer.Model.Tasks;
using TaskManager.DomainLayer.Infrastructure.Repositories;

namespace TaskManager.DomainLayer.Files
{
    internal class DefaultDevTasks
    {
        internal static void InsertDefaultTasks(SQLiteConnection connection)
        {
            InsertTaskIfNotExists(connection, "kaio", "Implementar Funcionalidade X", DateTime.Now.AddDays(7), "kaio");
            InsertTaskIfNotExists(connection, "kaio", "Corrigir bug Y", DateTime.Now.AddDays(3), "carolina");
            InsertTaskIfNotExists(connection, "kaio", "Implementar Funcionalidade Z", DateTime.Now.AddDays(10));
        }

        private static void InsertTaskIfNotExists(SQLiteConnection connection, string techLeaderLogin, string title, DateTime deadline, string developerLogin = null)
        {
            DevTaskRepo.InsertTaskIfNotExists
                (
                connection, 
                new DevTask
                    (
                        techLeaderLogin,
                        title,
                        deadline,
                        developerLogin
                    )
                );
        }
    }
}
