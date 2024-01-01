
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Service;

namespace TaskManager.DomainLayer.Model.People
{
    internal abstract class User : IUser
    {
        private string? _email;
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
        public JobEnum Job { get; private set; }
        public string? Email
        {
            get { return _email; }
            private set
            {
                if (IsValidEmail(value))
                {
                    _email = value;
                }
            }
        }

        [JsonConstructor]
        public User(string newName, string newLogin, string? newEmail = null)
        {
            Id = Math.Abs(Guid.NewGuid().GetHashCode());
            Name = newName;
            Email = newEmail;
            Login = newLogin;
            Password = HashPassword("123");
        }
        public bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Email inválido: {email}. Detalhes da exceção: {ex.Message}");
                return false;
            }
        }
        public void SetJob(JobEnum job)
        {
            Job = job;
        }
        public void ChangePassword(string currentPassword, string newPassword)
        {
            if (PasswordMatches(currentPassword))
            {
                SetPassword(newPassword);
                Message.PasswordChanged();
            }
        }
        private bool PasswordMatches(string password)
        {
            return Password == HashPassword(password);
        }
        private void SetPassword(string newPassword)
        {
            Password = HashPassword(newPassword);
        }
        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
        public void TryChangingPassword()
        {
            Title.ChangePassword();
            Console.WriteLine($"\nMudando a senha para o usuário {Name} ({Login})");

            string oldPassword = Authentication.ReadPassword("\nSenha antiga: ");

            if (oldPassword != null && Authentication.PasswordMatches(Password, oldPassword))
            {
                string newPassword = Authentication.ReadPassword("Senha nova: ");

                if (newPassword != null)
                {
                    ChangePassword(oldPassword, newPassword);
                }
            }
            else
            {
                Message.IncorrectPassword();
            }

            Console.ReadKey();
        }
        public abstract void Greeting();
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"\nId: {Id} \nName: {Name} \nLogin: {Login} \nJob: {Job}");
            if (Email != null)
            {
                sb.AppendLine($"\nEmail: {Email}");
            }
            return sb.ToString();
        }
    }
}
