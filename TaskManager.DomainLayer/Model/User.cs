
using System.Security.Cryptography;
using System.Text;

namespace TaskManager.DomainLayer.Model
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
        public abstract void Greeting();

        public void ChangePassword(string currentPassword, string newPassword)
        {
            if (PasswordMatches(currentPassword))
            {
                SetPassword(newPassword);
                Console.WriteLine("Password changed successfully.");
            }
            else
            {
                Console.WriteLine("Current password is incorrect. Password not changed.");
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
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"\nId: {Id} \nName: {Name} \nJob: {Job}");
            if (Email != null)
            {
                sb.AppendLine($"Email: {Email}");
            }
            return sb.ToString();
        }
    }
}
