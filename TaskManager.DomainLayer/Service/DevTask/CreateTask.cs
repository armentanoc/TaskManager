using System.Data.SQLite;
using System.Linq.Expressions;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Infrastructure.Repositories;
using TaskManager.DomainLayer.Model.People;
using TaskManager.DomainLayer.Model.Tasks;

namespace TaskManager.DomainLayer.Service.DevTask
{
    internal class CreateTask
    {
        public static void Execute(User _developer)
        {
            Title.NewTask();

            try
            {
                Console.Write("\nInforme o título para a nova DevTask: ");
                string title = Console.ReadLine();
                IsTitleNullOrWhitespace(title);

                Console.Write("\nInforme um descrição opcional (pressione Enter para pular): ");
                string description = Console.ReadLine();
                description = IsDescriptionNullOrWhitespace(description);

                Console.Write("\nInforme o login do líder técnico: ");
                string techLeaderLogin = Console.ReadLine();

                IsTechLeader(techLeaderLogin);
                CreateNewTask(title, description, techLeaderLogin, _developer);

            }
            catch (Exception ex)
            {
                Message.CatchException(ex);
            }
            finally
            {
                Message.PressAnyKeyToReturn();
            }
        }

        public static void ExecuteTechLeader(User _techLeader)
        {
            Title.NewTask();

            try
            {
                Console.Write("\nInforme o título para a nova DevTask: ");
                string title = Console.ReadLine();
                IsTitleNullOrWhitespace(title);

                Console.Write("\nInforme um descrição opcional (pressione Enter para pular): ");
                string description = Console.ReadLine();
                description = IsDescriptionNullOrWhitespace(description);

                Console.Write("\nInforme o login de quem irá assumir a tarefa: ");
                string developerLogin = Console.ReadLine();
                developerLogin = string.IsNullOrWhiteSpace(developerLogin) ? "TBD" : developerLogin;

                User developerInstance = null;

                if (DevTask.IsDeveloper(developerLogin) || developerLogin.Equals("TBD"))
                {
                    developerInstance = UserRepository.GetUsersList().FirstOrDefault(x => x.Login.Equals(developerLogin));
                }
                else
                {
                    Message.Error($"User {developerLogin} informado não é Developer.");
                }

                CreateNewTask(title, description, _techLeader.Login, developerInstance);
            }
            catch (Exception ex)
            {
                Message.CatchException(ex);
                Console.ReadKey();
            }
            finally
            {
                Message.PressAnyKeyToReturn();
            }
        }
        private static string IsDescriptionNullOrWhitespace(string? description)
        {
            return string.IsNullOrWhiteSpace(description) ? "TBD" : description;
        }
        private static void CreateNewTask(string title, string? description, string? techLeaderLogin, User developer)
        {
            string developerLogin;

            if (developer == null)
            {
                developerLogin = "TBD";
            }
            else
            {
                developerLogin = developer.Login;
            }

            DevTask newDevTask = new DevTask(
                    techLeaderLogin: techLeaderLogin,
                    title: title,
                    developerLogin: developerLogin,
                    description: description
                );
            DevTaskRepository.InitializeNewDevTask(newDevTask);
            Console.ReadKey();
        }
        private static void IsTechLeader(string? techLeaderLogin)
        {
            if (!DevTask.IsTechLeader(techLeaderLogin))
            {
                throw new ArgumentException("A pessoa Tech Leader especificada não existe. A tarefa não será criada.");
            }
        }
        private static void IsDeveloper(string? developerLogin)
        {
            if (!DevTask.IsDeveloper(developerLogin) && !string.IsNullOrWhiteSpace(developerLogin))
            {
                throw new ArgumentException("A pessoa Desenvolvedora especificada não existe. A tarefa não será criada.");
            }
        }
        private static void IsTitleNullOrWhitespace(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("O título não pode ser nulo ou vazio");
            }
        }
    }
}