using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Infrastructure.Repositories;
using TaskManager.DomainLayer.Model.People;
using TaskManager.DomainLayer.Model.Tasks;

namespace TaskManager.DomainLayer.Service.Tasks
{
    internal class ApproveTask
    {
        internal static void Execute(User techLeader)
        {
            DevTaskRepository.DisplayTasksByTeam(techLeader.Login);

            Title.ApproveTask();
            Console.Write("\n\nInforme o ID da tarefa que deseja aprovar: ");
            string taskId = Console.ReadLine();

            if (TryToApproveTask(taskId, techLeader))
            {
                Message.LogAndConsoleWrite($"Operação de aprovação efetuada com sucesso.");
                Message.PressAnyKeyToContinue();
            }
            else
            {
                Message.LogAndConsoleWrite("\nNão foi possível aprovar a tarefa. Verifique o ID ou se você é o líder técnico associado.");
                Message.PressAnyKeyToContinue();
            }

        }
        private static bool TryToApproveTask(string taskId, User techLeader)
        {
            var taskToApprove =
                DevTaskRepository
                .GetTaskList()
                .FirstOrDefault(
                    task =>
                        task.Id.Equals(taskId)
                        && task.TechLeaderLogin.Equals(techLeader.Login)
                        && task.RequiresApprovalToComplete.Equals(true)
                );

            if (taskToApprove != null)
            {
                taskToApprove.SetRequiresApprovalToComplete(false);
                DevTaskRepository.ApproveTaskById(taskToApprove, techLeader);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
