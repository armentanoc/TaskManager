using TaskManager.DomainLayer.Infrastructure.Repositories;
using TaskManager.DomainLayer.Model.People;

namespace TaskManager.DomainLayer.Service.Relationships
{
    internal class GetTeamStatistics
    {
        internal static void Execute(User techLeader)
        {
            var teamTaskList = DevTaskRepo.GetTeamTaskList(techLeader.Login);
        }
    }
}