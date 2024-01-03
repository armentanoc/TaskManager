using System.Data.SQLite;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Model.People;
using TaskManager.DomainLayer.Model.Tasks;
using TaskManager.DomainLayer.Repositories;

namespace TaskManager.DomainLayer.Service
{
    internal class CreateDevTask
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

                CreateTask(_developer, title, description, techLeaderLogin);
            }
            catch (Exception ex)
            {
                Title.Error();
                Message.CatchException(ex);
            }
            finally
            {
                Console.WriteLine("\nPressione qualquer tecla para retornar. ");
                Console.ReadKey();
            }
        }

        private static string IsDescriptionNullOrWhitespace(string? description)
        {
            return string.IsNullOrWhiteSpace(description) ? "TBD" : description;
        }

        private static void CreateTask(User developer, string title, string? description, string? techLeaderLogin)
        {
            DevTask newDevTask = new DevTask(
                    techLeaderLogin: techLeaderLogin,
                    title: title,
                    developerLogin: developer.Login,
                    description: description
                );
            DevTaskRepository.InitializeNewDevTask(newDevTask);
            Console.WriteLine($"\nDevTask criada com sucesso: \n{newDevTask.Title}.");
        }

        private static void IsTechLeader(string? techLeaderLogin)
        {
            if (!DevTask.IsTechLeader(techLeaderLogin))
            {
                throw new ArgumentException("A pessoa Tech Leader especificada não existe. A tarefa não será criada.");
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