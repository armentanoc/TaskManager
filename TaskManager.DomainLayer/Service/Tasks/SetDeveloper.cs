
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Infrastructure.Repositories;
using TaskManager.DomainLayer.Model.People;
using TaskManager.DomainLayer.Model.Tasks;

namespace TaskManager.DomainLayer.Service.Tasks
{
    internal class SetDeveloper
    {
        internal static void Execute(User techLeader)
        {
            try
            {
                Console.Clear();

                DevTaskRepo.DisplayTasksByTeam(techLeader.Login);
                Title.SetDeveloper();

                Console.Write("\n\nInforme o ID da tarefa que deseja modificar: ");
                string? taskId = Console.ReadLine();

                UserRepo.DisplayDevelopers();
                Title.SetDeveloper();

                Console.Write("\n\nInforme o Login do Dev que irá receber a tarefa: ");
                string? devLogin = Console.ReadLine();

                if (TrySettingDeveloperToTask(taskId, devLogin, techLeader))
                {
                    Message.LogAndConsoleWrite($"Operação efetuada com sucesso.");
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

        private static bool TrySettingDeveloperToTask(string? taskId, string? devId, User techLeader)
        {
            var taskToAlter = IsTaskAppropriate(taskId, techLeader);
            var developerLogin = IsDevAppropriate(devId, techLeader);

            taskToAlter.SetDeveloper(developerLogin);
            DevTaskRepo.UpdateTaskDeveloperLoginById(taskToAlter, developerLogin, techLeader.Login);
            return true;
        }
        private static DevTask IsTaskAppropriate(string taskId, User techLeader)
        {
            var taskToAlter =
                DevTaskRepo
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
                throw new ArgumentException("Não foi possível atualizar o Dev da tarefa. Verifique o ID ou se você é o Tech Leader associado.");
            }
        }
        private static string IsDevAppropriate(string devLogin, User techLeader)
        {
            var dev =
                UserRepo
                .GetDevelopersInDatabase()
                .FirstOrDefault(
                dev =>
                        dev.Login.Equals(devLogin)
                );

            if (dev is User)
            {
                return dev.Login;
            }
            else
            {
                throw new ArgumentException($"Usuário '{devLogin}' não encontrado.");
            }
        }
    }
}
