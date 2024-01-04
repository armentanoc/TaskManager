using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Infrastructure.Repositories;
using TaskManager.DomainLayer.Model.People;
using TaskManager.DomainLayer.Model.Tasks;

namespace TaskManager.DomainLayer.Service.Relationships
{
    internal class GetTeamStatistics
    {
        private readonly static List<StatusEnum> _statuses = new List<StatusEnum>
        {
            StatusEnum.EmAnaliseParaBacklog,
            StatusEnum.Backlog,
            StatusEnum.EmProgresso,
            StatusEnum.EmAnaliseParaConclusao,
            StatusEnum.Concluida,
            StatusEnum.Cancelada
        };
        internal static void Execute(User techLeader)
        {
            var teamTaskList = DevTaskRepo.GetTeamTaskList(techLeader.Login);
            DisplayStatusStatistics(teamTaskList);
        }
        private static void DisplayStatusStatistics(IEnumerable<DevTask> teamTaskList)
        {
            Title.TeamStatistics();

            int totalTasks = teamTaskList.Count();

            foreach (StatusEnum status in _statuses)
            {
                int count = teamTaskList.Count(task => task.Status == status);
                double percentage = (double)count / totalTasks * 100;
                Console.WriteLine($"{status}: {count} ocorrências, {percentage:F2}%");
            }

            Message.PressAnyKeyToContinue();
        }
    }
}
