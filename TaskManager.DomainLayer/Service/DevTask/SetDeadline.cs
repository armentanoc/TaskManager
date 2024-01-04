
using System.Globalization;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Infrastructure.Repositories;
using TaskManager.DomainLayer.Model.People;
using TaskManager.DomainLayer.Model.Tasks;

namespace TaskManager.DomainLayer.Service.DevTask
{
    internal class SetDeadline
    {
        internal static void Execute(User techLeader)
        {
            try
            {
                Console.Clear();
                DevTaskRepository.DisplayTasksByTeam(techLeader.Login);
                Title.SetDeadline();

                Console.Write("\n\nInforme o ID da tarefa cuja Deadline deseja modificar: ");
                string taskId = Console.ReadLine();

                if (TrySettingDeadline(taskId, techLeader))
                {
                    Message.LogAndConsoleWrite($"Operação de alteração de Deadline efetuada com sucesso.");
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

        private static bool TrySettingDeadline(string taskId, User techLeader)
        {
            var taskToAlter = IsTaskAppropriate(taskId, techLeader);
            DateTime deadline = GetDeadline();
            taskToAlter.SetDeadline(deadline);
            DevTaskRepository.UpdateTaskDeadlineById(taskToAlter, techLeader);
            return true;
        }

        private static DevTask IsTaskAppropriate(string taskId, User techLeader)
        {
            var taskToAlter =
                DevTaskRepository
                .GetTaskList()
                .FirstOrDefault(
            task =>
                        task.Id.Equals(taskId)
                        && task.TechLeaderLogin.Equals(techLeader.Login)
                );

            if (taskToAlter is DevTask)
            {
                return taskToAlter;
            }
            else
            {
                throw new ArgumentException("Não foi possível atualizar o Deadline da tarefa. Verifique o ID ou se você é o dev associado.");
            }
        }
        private static DateTime GetDeadline()
        {
            Console.Write("\nInforme para qual data e horário deseja alterar a tarefa (DD/MM/YYYY HH:mm): ");
            string dateTimeString = Console.ReadLine();

            if (DateTime.TryParseExact(dateTimeString, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime deadline))
            {
                Message.LogWrite($"Data e horário informados: {deadline:dd/MM/yyyy HH:mm}");
                return deadline;
            }
            else
            {
                throw new ArgumentException("Formato de data e horário inválido. Utilize o formato DD/MM/YYYY HH:mm");
            }
        }
    }
}
