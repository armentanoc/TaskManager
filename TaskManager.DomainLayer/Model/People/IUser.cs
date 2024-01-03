using TaskManager.ConsoleInteraction.Components;

namespace TaskManager.DomainLayer.Model.People
{
    internal interface IUser
    {
        public bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address.Equals(email);
            }
            catch (FormatException ex)
            {
                Message.Error($"Email inválido: {email}. Detalhes da exceção: {ex.Message}");
                return false;
            }
        }
        void Greeting();
    }
}