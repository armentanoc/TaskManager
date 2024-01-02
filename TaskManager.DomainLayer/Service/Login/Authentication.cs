
using System.Security.Cryptography;
using System.Text;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Model.People;
using TaskManager.DomainLayer.Repositories;

namespace TaskManager.DomainLayer.Service.Login
{
    internal class Authentication
    {
        public static User? Authenticate()
        {
            Title.Login();

            string login = ReadLogin();
            string password = ReadPassword("Senha: ");

            if (password == null)
            {
                Message.AuthenticationFailed();
                return null;
            }

            var user = ValidateUser(login, password);

            return user;
        }

        private static string ReadLogin()
        {
            Console.Write("\nUsuário: ");
            return Console.ReadLine();
        }

        internal static string ReadPassword(string prompt)
        {
            const int MinPasswordLength = 3;

            Console.Write(prompt);
            var passwordBuilder = new StringBuilder();

            try
            {
                do
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                    else if (key.Key == ConsoleKey.Backspace)
                    {
                        if (passwordBuilder.Length > 0)
                        {
                            passwordBuilder.Length--;
                            Console.Write("\b \b");
                        }
                    }
                    else if (key.Key == ConsoleKey.Escape)
                    {
                        Console.WriteLine("\nPassword input canceled.");
                        return null;
                    }
                    else if (!char.IsControl(key.KeyChar))
                    {
                        passwordBuilder.Append(key.KeyChar);
                        Console.Write("*");
                    }
                } while (true);

                Console.WriteLine();

                string password = passwordBuilder.ToString();

                if (password.Length < MinPasswordLength)
                {
                    Message.SmallPassword();
                    return null;
                }

                return password;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
                return null;
            }
        }

        private static User ValidateUser(string login, string enteredPassword)
        {
           var user = UserRepository.GetUserByLogin(login);

            if (user == null)
            {
                Message.IncorrectUser();
                return null;
            }

            if (PasswordMatches(user.Password, enteredPassword))
            {
                user.Greeting();
                return user;
            }
            else
            {
                Message.AuthenticationFailed();
                return null;
            }
        }

        internal static bool PasswordMatches(string storedHashedPassword, string enteredPassword)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] enteredHashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));
                string enteredHashedPassword = BitConverter.ToString(enteredHashedBytes).Replace("-", "").ToLower();

                return storedHashedPassword.Equals(enteredHashedPassword);
            }
        }
    }
}
