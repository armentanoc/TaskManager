using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Infrastructure.Repositories;
using TaskManager.DomainLayer.Model.People;
using TaskManager.DomainLayer.Model.Tasks;

namespace TaskManager.DomainLayer.Service.DevTask
{
    internal class UpdateStatus
    {
        internal static void Execute(User developer)
        {
            try
            {
                Console.Clear();
                DevTaskRepository.DisplayTasksByDeveloper(developer.Login);
                Title.UpdateTask();
                Console.Write("\n\nInforme o ID da tarefa cujo status deseja modificar: ");
                string taskId = Console.ReadLine();

                if (TryToUpdateTask(taskId, developer))
                {
                    Message.LogAndConsoleWrite($"Operação de alteração de status efetuada com sucesso.");
                    Message.PressAnyKeyToReturn();
                }
                else
                {
                    Message.LogAndConsoleWrite("\nNão foi possível atualizar o status da tarefa. Verifique o ID ou se você é o dev associado.");
                    Message.PressAnyKeyToReturn();
                }
            }
            catch (Exception ex)
            {
                Message.CatchException(ex);
            }
            finally
            {
                Console.ReadKey();
            }
        }

        internal static void ExecuteTechLeader(User techLeader)
        {
            try
            {
                Console.Clear();
                DevTaskRepository.DisplayTasksByTeam(techLeader.Login);
                Title.UpdateTask();
                Console.Write("\n\nInforme o ID da tarefa cujo status deseja modificar: ");
                string taskId = Console.ReadLine();

                if (TryToUpdateTaskTechLeader(taskId, techLeader))
                {
                    Message.LogAndConsoleWrite($"Operação de alteração de status efetuada com sucesso.");
                    Message.PressAnyKeyToReturn();
                }
                else
                {
                    Message.LogAndConsoleWrite("\nNão foi possível atualizar o status da tarefa. Verifique o ID ou se você é o dev associado.");
                    Message.PressAnyKeyToReturn();
                }
            }
            catch (Exception ex)
            {
                Message.CatchException(ex);
            }
            finally
            {
                Console.ReadKey();
            }
        }
        private static bool TryToUpdateTask(string taskId, User developer)
        {
            var taskToAlter =
                DevTaskRepository
                .GetTaskList()
                .FirstOrDefault(
                    task =>
                        task.Id.Equals(taskId)
                        && task.DeveloperLogin.Equals(developer.Login)
                );

            if (taskToAlter != null)
            {
                StatusEnum status = GetStatus();
                taskToAlter.SetStatus(status);
                DevTaskRepository.UpdateTaskDeadlineById(taskToAlter, developer);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool TryToUpdateTaskTechLeader(string taskId, User techLeader)
        {
            var taskToAlter =
                DevTaskRepository
                .GetTaskList()
                .FirstOrDefault(
                    task =>
                        task.Id.Equals(taskId)
                        && task.TechLeaderLogin.Equals(techLeader.Login)
                );

            if (taskToAlter != null)
            {
                StatusEnum status = GetStatus();
                taskToAlter.SetStatusTechLeader(status);
                DevTaskRepository.UpdateTaskStatusByIdFromTechLeader(taskToAlter, techLeader);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static StatusEnum GetStatus()
        {
            Console.Write("\nInforme para qual status deseja alterar a tarefa (EmAnaliseParaBacklog," +
                " Backlog, EmProgresso, EmAnaliseParaConclusao, Concluida, Cancelada): ");
            string status = Console.ReadLine();

            if (Enum.TryParse(status, true, out StatusEnum statusEnum))
            {
                Message.LogWrite($"Valor {statusEnum} corresponde a um StatusEnum");
                return statusEnum;
            }
            else
            {
                throw new ArgumentException("O valor informado não corresponde a relação de ParentChild ou Dependency");
            }
        }
    }
}
