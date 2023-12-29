
using System.Security.Cryptography;
using System.Text;
using TaskManager.ConsoleInteraction.Components;
using TaskManager.DomainLayer.Model;

namespace TaskManager.DomainLayer.Service
{
    internal class Login
    {
        public static User? Authenticate(List<User> users)
        {
            Title.Login();

            string login = ReadLogin();
            string password = ReadPassword();

            if (password == null)
            {
                Message.AuthenticationFailed();
                return null;
            }

            var user = ValidateUser(users, login, password);

            return user;
        }

        private static string ReadLogin()
        {
            Console.Write("\nLogin: ");
            return Console.ReadLine();
        }

        private static string ReadPassword()
        {
            const int MinPasswordLength = 3;

            Console.Write("Senha: ");
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
                    Console.WriteLine("Password should have at least 3 characters.");
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

        private static User ValidateUser(List<User> users, string? login, string? password)
        {
            if (login == null || password == null)
            {
                // Handle the case where login or password is null
                Message.AuthenticationFailed();
                return null;
            }

            var user = users.Find(u => u.Login == login);

            if (user != null && user.Password != null && PasswordMatches(user.Password, password))
            {
                user.Greeting();
                return user;
            }
            else
            {
                Message.IncorrectPassword();
                return null;
            }
        }

        private static bool PasswordMatches(string storedHashedPassword, string enteredPassword)
        {
            // Compare the hashed password with the entered password
            using (var sha256 = SHA256.Create())
            {
                byte[] enteredHashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));
                string enteredHashedPassword = BitConverter.ToString(enteredHashedBytes).Replace("-", "").ToLower();

                return storedHashedPassword.Equals(enteredHashedPassword);
            }
        }
    }
}
