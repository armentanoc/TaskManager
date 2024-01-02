
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Repositories;
using TaskManager.DomainLayer.Service.Login;

namespace TaskManager.DomainLayer.Model.People
{
    public class User : IUser
    {
        public string Id { get; private set; }
        private string? _email;
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
            Name = newName;
            Email = newEmail;
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
                Console.WriteLine("\nSenha alterada com sucesso.");
            }
        }
        private bool PasswordMatches(string password)
        {
            return Password == HashPassword(password);
        }
        private void SetPassword(string newPassword)
        {
            Password = HashPassword(newPassword);
            UserRepository.UpdatePasswordById(Id, Password);
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
        public virtual void Greeting()
        {
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"\nName: {Name} \nLogin: {Login} \nJob: {Job}");
            if (Email != null)
            {
                sb.AppendLine($"\nEmail: {Email}");
            }
            return sb.ToString();
        }
    }
}
