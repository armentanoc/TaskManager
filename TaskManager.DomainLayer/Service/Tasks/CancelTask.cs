using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Infrastructure.Repositories;
using TaskManager.DomainLayer.Model.People;
using TaskManager.DomainLayer.Model.Tasks;

namespace TaskManager.DomainLayer.Service.Tasks
{
    internal class CancelTask
    {
        internal static void Execute(User techLeader)
        {
            DevTaskRepository.DisplayTasksByTeam(techLeader.Login);

            Title.CancelTask();
            Message.LogAndConsoleWrite("\n\nInforme o ID da tarefa que deseja cancelar: ");
            string taskId = Console.ReadLine();

            if (TryCancelTask(taskId, techLeader))
            {
                Message.LogAndConsoleWrite($"Operação de cancelamento efetuada com sucesso.");
                Message.PressAnyKeyToContinue();
            }
            else
            {
                Message.LogAndConsoleWrite("\nNão foi possível cancelar a tarefa. Verifique o ID ou se você é o líder técnico associado.");
                Message.PressAnyKeyToContinue();
            }

        }
        private static bool TryCancelTask(string taskId, User techLeader)
        {
            var taskToCancel =
                DevTaskRepository
                .GetTaskList()
                .FirstOrDefault(
                    task =>
                        task.Id.Equals(taskId)
                        && task.TechLeaderLogin.Equals(techLeader.Login)
                        && !task.Status.Equals(StatusEnum.Cancelada)
                );

            if (taskToCancel != null)
            {
                taskToCancel.SetStatus(StatusEnum.Cancelada);
                DevTaskRepository.CancelTaskById(taskToCancel, techLeader);
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
