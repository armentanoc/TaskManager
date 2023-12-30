
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Model.Tasks;

namespace TaskManager.DomainLayer.Repositories
{
    internal static class DevTaskRepository
    {
        public static List<DevTask> taskList;

        static DevTaskRepository()
        {
            InitializeDefaultTasks();
        }

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

        public static List<DevTask> All()
        {
            return taskList;
        }

        public static void DisplayAll()
        {
            Console.Clear();
            Title.AllTasks();
            try
            {
                foreach (var task in taskList)
                {
                    task.ToStringPrint();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro ao exibir as tarefas: {ex.Message}");
            }
        }
    }
}
