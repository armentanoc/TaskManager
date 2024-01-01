
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Model.Tasks;

namespace TaskManager.DomainLayer.Repositories
{
    internal static class DevTaskRepository
    {
        public static List<DevTask>? taskList;

        static DevTaskRepository() => InitializeDefaultTasks();

        private static void InitializeDefaultTasks()
        {
            taskList = new List<DevTask>
            {
                new DevTask(
                    techLeaderLogin : "kaio", 
                    title : "Implement Feature X", 
                    deadline : DateTime.Now.AddDays(7),
                    developerLogin : "kaio"
                    ),
                new DevTask(
                    techLeaderLogin : "kaio",
                    title : "Fix bug Y",
                    deadline : DateTime.Now.AddDays(3),
                    developerLogin : "carolina"
                    ),
                new DevTask(
                    techLeaderLogin : "kaio",
                    title : "Implement Feature Z",
                    deadline : DateTime.Now.AddDays(10)
                    ),
            };
        }

        public static List<DevTask>? All()
        {
            return taskList;
        }

        public static void DisplayAll()
        {
            Console.Clear();
            Title.AllTasks();
            DisplayTasks(taskList);
        }

        public static void DisplayTasksByDeveloper(string developerLogin)
        {
            Console.Clear();
            Title.DeveloperTasks(developerLogin);
            var tasksForDeveloper = taskList.Where(task => task.DeveloperLogin == developerLogin).ToList();
            DisplayTasks(tasksForDeveloper);
            Message.PressAnyKeyToReturn();
        }

        private static void DisplayTasks(IEnumerable<DevTask> tasks)
        {
            try
            {
                foreach (var task in tasks)
                {
                    task.ToStringPrint();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro ao exibir as tarefas: {ex.Message}");
            }
        }

        internal static void DisplayTeamTasksByTechLeader(string techLeaderLogin)
        {
            Console.Clear();
            Title.TeamTasks(techLeaderLogin);
            var tasksForTechLeader = taskList.Where(task => task.TechLeaderLogin == techLeaderLogin).ToList();
            DisplayTasks(tasksForTechLeader);
            Message.PressAnyKeyToContinue();
        }

        internal static void CancelTask(string techLeaderLogin)
        {
            DisplayTeamTasksByTechLeader(techLeaderLogin);

            Title.CancelTask();
            Console.Write("\n\nInforme o ID da tarefa que deseja cancelar: ");
            if (int.TryParse(Console.ReadLine(), out int taskId) && TryCancelTask(taskId, techLeaderLogin))
            {
                Console.WriteLine($"\nTarefa {taskId} cancelada com sucesso.");
            }
            else
            {
                Console.WriteLine("\nNão foi possível cancelar a tarefa. Verifique o ID ou se você é o líder técnico associado.");
            }

            Message.PressAnyKeyToReturn();
        }

        private static bool TryCancelTask(int taskId, string techLeaderLogin)
        {
            var taskToCancel = taskList.FirstOrDefault(
        task =>
            task.Id.Equals(taskId)
            && task.TechLeaderLogin.Equals(techLeaderLogin)
            && !task.Status.Equals(StatusEnum.Cancelada)
    );

            if (taskToCancel != null)
            {
                taskToCancel.SetStatus(StatusEnum.Cancelada);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
