
using System.Globalization;
using TaskManager.DomainLayer.Model.People;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Infrastructure.Repositories;

namespace TaskManager.DomainLayer.Model.Tasks
{
    public class DevTask
    {
        public string Id { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public string TechLeaderLogin { get; private set; }
        public string DeveloperLogin { get; private set; }
        public StatusEnum Status { get; private set; }
        public bool RequiresApprovalToComplete { get; private set; }
        public DateTime Deadline { get; private set; }
        public DateTime? CompletionDateTime { get; private set; }

        //general constructor to recovery information

        public DevTask(
            string id,
            string title,
            string description,
            string techLeaderLogin,
            string developerLogin,
            StatusEnum status,
            bool requiresApprovalToComplete,
            DateTime deadline,
            DateTime completionDateTime)
        {
            Id = id;
            Title = title;
            Description = description ?? string.Empty;
            TechLeaderLogin = techLeaderLogin;
            DeveloperLogin = developerLogin;
            Status = status;
            RequiresApprovalToComplete = requiresApprovalToComplete;
            Deadline = deadline;
            CompletionDateTime = completionDateTime;
        }

        //common constructor
        public DevTask(string techLeaderLogin, string title, DateTime deadline, string description = null, string developerLogin = null)
        {
            ValidateTechLeader(techLeaderLogin);
            ValidateDeveloper(developerLogin);
            ValidateDeadline(deadline);

            TechLeaderLogin = techLeaderLogin;
            Title = title;
            Description = description ?? string.Empty;
            DeveloperLogin = developerLogin ?? string.Empty;
            Deadline = deadline;
                                            
            if (IsTechLeader(developerLogin))
            {
                Status = StatusEnum.Backlog;
                RequiresApprovalToComplete = false;
            } else
            {
                RequiresApprovalToComplete = true;
                Status = StatusEnum.EmAnaliseParaBacklog;
            }
        }
          
        //task created by developer
        public DevTask(string developerLogin, string techLeaderLogin, string title, string description = null)
        {
            ValidateTechLeader(techLeaderLogin);
            ValidateDeveloper(developerLogin);

            DeveloperLogin = developerLogin;
            TechLeaderLogin = techLeaderLogin;
            Title = title;
            Description = description ?? string.Empty;

            RequiresApprovalToComplete = true;
            Status = StatusEnum.EmAnaliseParaBacklog;
        }

        // validations                        
        private void ValidateTechLeader(string techLeaderLogin)
        {
            try
            {
                if (!IsTechLeader(techLeaderLogin))
                {
                    throw new ArgumentException("A pessoa Tech Leader especificada não existe. A tarefa não será criada.");
                }
            }
            catch (Exception ex)
            {
                Message.CatchException(ex);
            }
        }
        private void ValidateDeveloper(string developerLogin)
        {
            try
            {
                if (developerLogin != null && !IsDeveloper(developerLogin) && !IsTechLeader(developerLogin))
                {
                    throw new ArgumentException("A pessoa Desenvolvedora especificada não existe. Operação não será concluída.");
                }
            }
            catch (Exception ex)
            {
                Message.CatchException(ex);
            }
        }
        static internal bool IsDeveloper(string developerLogin)
        {
            return UserRepository.GetUsersList().Any(user => user.Login == developerLogin && user.Job == JobEnum.Developer);
        }
        static internal bool IsTechLeader(string techLeaderLogin)
        {
            return UserRepository.GetUsersList().Any(user => user.Login == techLeaderLogin && user.Job == JobEnum.TechLeader);
        }

        //tech leader methods
        public void SetDeadline(DateTime deadline)
        {
            ValidateDeadline(deadline);
            Deadline = deadline;
        }
        private void ValidateDeadline(DateTime deadline)
        {
            if (deadline <= DateTime.Now.AddHours(2))
            {
                throw new ArgumentException("A data limite deve ser pelo menos 2 horas no futuro.");
            }
        }
        public void SetDeveloper(string developerLogin)
        {
            ValidateDeveloper(developerLogin);
            DeveloperLogin = developerLogin;
        }
        public void SetRequiresApprovalToComplete(bool requiresApproval)
        {
            RequiresApprovalToComplete = requiresApproval;
        }

        //common methods
        public void SetStatus(StatusEnum status)
        {
            if (status == StatusEnum.Concluida)
            {
                if (RequiresApprovalToComplete)
                {
                    throw new InvalidOperationException("A tarefa não pode ser marcada como concluída sem aprovação.");
                }
                else
                {
                    CompletionDateTime = DateTime.Now;
                }
            }
            Status = status;
        }
        public void SetDescription(string description)
        {
            Description = description;
        }

        //toString
        private void FormatDeveloper()
        {
            if (DeveloperLogin != null)
            {
                Console.WriteLine($"Desenvolvedor: {DeveloperLogin}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Desenvolvedor: TBD");
                Console.ResetColor();
            }
        }
        private void FormatStatus()
        {
            ConsoleColor statusColor;

            if (Status.Equals(StatusEnum.Concluida))
            {
                statusColor = ConsoleColor.Green;
            }
            else if (new List<StatusEnum> { StatusEnum.Atrasada, StatusEnum.Cancelada }.Contains(Status))
            {
                statusColor = ConsoleColor.Red;
            }
            else if (Status.Equals(StatusEnum.EmAnaliseParaBacklog))
            {
                statusColor = ConsoleColor.Cyan;
            }
            else
            {
                statusColor = ConsoleColor.Yellow;
            }
            Console.ForegroundColor = statusColor;
            Console.WriteLine($"Status: {Status}");
            Console.ResetColor();
        }
        private void FormatCompletionDateTime()
        {
            if (CompletionDateTime.HasValue && CompletionDateTime.Value != DateTime.MinValue)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                string completionDateTimeString = CompletionDateTime.Value.ToString("g", CultureInfo.GetCultureInfo("pt-BR"));
                Console.WriteLine($"Data de Conclusão: {completionDateTimeString}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Data de Conclusão: TBD");
            }
            Console.ResetColor();
        }
        private void FormatDeadline()
        {
            if (Deadline.Equals(DateTime.MinValue))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Prazo: TBD pela pessoa Tech Leader.");
            }
            else
            {
                Console.ForegroundColor = Deadline < DateTime.Now ? ConsoleColor.Red : ConsoleColor.Gray;
                string deadlineString = Deadline.ToString("g", CultureInfo.GetCultureInfo("pt-BR"));
                Console.WriteLine($"Prazo: {deadlineString}");
            }

            Console.ResetColor();
        }
        private void FormatRequiresApprovalToComplete()
        {
            Console.ForegroundColor = RequiresApprovalToComplete ? ConsoleColor.Yellow : ConsoleColor.Gray;
            Console.WriteLine($"Requer aprovação para completar: {RequiresApprovalToComplete}");
            Console.ResetColor();
        }
        public void ToStringPrint()
        {
            Console.WriteLine($"\n" +
                $"ID: {Id}\n" +
                $"Título: {Title}\n" +
                $"Descrição: {Description ?? "TBD"}\n" +
                $"Tech Leader: {TechLeaderLogin}");

            FormatDeveloper();
            FormatStatus();
            FormatRequiresApprovalToComplete();
            FormatDeadline();
            FormatCompletionDateTime();
        }
    }
}
