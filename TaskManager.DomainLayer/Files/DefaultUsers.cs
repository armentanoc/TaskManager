
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Model.People;
using TaskManager.DomainLayer.Infrastructure.Repositories;
using System.Data.SQLite;

namespace TaskManager.DomainLayer.Files
{
    public class DefaultUsers
    {
        internal static void InsertDefaultUsersIntoTable(SQLiteConnection connection)
        {
            var newUsers = new List<User>
            {
                new TechLeader("Kaio", "kaio"),
                new Developer("Ana Carolina", "carolina"),
            };

            UserRepo.InsertUsersIfNotExist(connection, newUsers);
        }
    }
}
