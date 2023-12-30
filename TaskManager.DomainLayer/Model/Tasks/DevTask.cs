
using System.Text;
using TaskManager.DomainLayer.Model.People;
using TaskManager.DomainLayer.Repositories;

namespace TaskManager.DomainLayer.Model.Tasks
{
    internal class DevTask
    {
        public readonly int Id;
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string TechLeaderLogin { get; private set; }
        public string DeveloperLogin { get; private set; }
        public StatusEnum Status { get; private set; }
        public bool RequiresApprovalToComplete { get; private set; }
        public DateTime Deadline { get; private set; }
        public DateTime? CompletionDateTime { get; private set; }
        public DevTask(string techLeaderLogin, string title, DateTime deadline, string description = null, string developerLogin = null)
        {
            ValidateTechLeader(techLeaderLogin);
            ValidateDeveloper(developerLogin);
            ValidateDeadline(deadline);

            Id = Math.Abs(DateTime.Now.GetHashCode());

            TechLeaderLogin = techLeaderLogin;
            Title = title;
            Description = description;
            DeveloperLogin = developerLogin;
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
        
        // validations                        
        private void ValidateTechLeader(string techLeaderLogin)
        {
            if (!IsTechLeader(techLeaderLogin))
            {
                throw new ArgumentException("A pessoa Tech Leader especificada não existe. A tarefa não será criada.");
            }
        }
        private void ValidateDeveloper(string developerLogin)
        {
            if (developerLogin != null && !IsDeveloper(developerLogin) && !IsTechLeader(developerLogin))
            {
                throw new ArgumentException("A pessoa Desenvolvedora especificada não existe. Operação não será concluída.");
            }
        }
        private bool IsDeveloper(string developerLogin)
        {
            return UserRepository.userList.Any(user => user.Login == developerLogin && user.Job == JobEnum.Developer);
        }
        private bool IsTechLeader(string techLeaderLogin)
        {
            return UserRepository.userList.Any(user => user.Login == techLeaderLogin && user.Job == JobEnum.TechLeader);
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
                Console.WriteLine("Desenvolvedor: Não atribuído");
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
            else if (Status.Equals(StatusEnum.Atrasada))
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
            Console.WriteLine
                (
                CompletionDateTime.HasValue && CompletionDateTime.Value != DateTime.MinValue
                ? $"Data de Conclusão: {CompletionDateTime}"
                : "Data de Conclusão: Não concluída"
                );
        }
        public void ToStringPrint()
        {

            Console.WriteLine($"\n" +
                $"ID: {Id}\n" +
                $"Título: {Title}\n" +
                $"Descrição: {Description}\n" +
                $"Tech Leader: {TechLeaderLogin}");

            FormatDeveloper();
            FormatStatus();

            Console.WriteLine
                ($"Requer aprovação para completar: {RequiresApprovalToComplete}\n" +
                $"Prazo: {Deadline}");

            FormatCompletionDateTime();
        }
    }
}
