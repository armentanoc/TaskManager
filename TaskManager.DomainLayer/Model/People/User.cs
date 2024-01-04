
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Infrastructure.Repositories;
using TaskManager.DomainLayer.Service.Login;
using TaskManager.Logger;

namespace TaskManager.DomainLayer.Model.People
{
    public class User : IUser
    {
        public static LogWriter _logWriter { get; private set; }
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
        public JobEnum Job { get; private set; }
        public string? Email { get; private set; }

        [JsonConstructor]
        public User(string newName, string newLogin, string? newEmail = null)
        {
            Name = newName;
            TryChangingEmail(newEmail);
            Login = newLogin;
            Password = HashPassword("1234");
        }

        public User(string id, string name, string login, string password, string email, JobEnum job)
        {
            Id = id;
            Name = name;
            Login = login;
            Email = email;
            Password = password;
            Job = job;
        }

        // virtual
        public virtual void Greeting() { }

        // setters
        public void SetJob(JobEnum job) => Job = job;

        // password change methods
        private void TryChangingEmail(string? newEmail)
        {
            if (IsValidEmail(newEmail))
            {
                Email = newEmail;
            }
            else
            {
                Email = string.Empty;
                _logWriter = new LogWriter($"Email {newEmail} não é válido. String vazia atribuída.");
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

                if (!string.IsNullOrWhiteSpace(newPassword))
                {
                    ChangePassword(oldPassword, newPassword);
                }
                else
                {
                    Message.PasswordIsNullOrWhitespace();
                }
            }
            else
            {
                Message.IncorrectPassword();
            }

            Message.PressAnyKeyToContinue();
        }
        public void ChangePassword(string currentPassword, string newPassword)
        {
            if (IsThisTheUserPassword(currentPassword))
            {
                SetPassword(newPassword);
            }
        }
        private void SetPassword(string newPassword)
        {
            Password = HashPassword(newPassword);
            UserRepo.UpdatePasswordById(this, Password);
        }

        // helpers and validation
        internal bool IsThisTheUserPassword(string? password)
        {
            return Password.Equals(HashPassword(password));
        }
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
                string prompt = $"Email inválido: {email}. Detalhes da exceção: {ex.Message}";
                Message.Error(prompt);
                return false;
            }
        }
        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"\nName: {Name} \nLogin: {Login} \nJob: {Job}");
            if (!string.IsNullOrWhiteSpace(Email))
            {
                sb.AppendLine($"\nEmail: {Email}");
            }
            return sb.ToString();
        }
        }
    }

