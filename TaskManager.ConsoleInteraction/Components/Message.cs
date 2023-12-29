



namespace TaskManager.ConsoleInteraction.Components
{
    public class Message
    {
        public static void AuthenticationFailed()
        {

            Title.Error();
            Console.WriteLine("\nFalha na autenticação: senha deve ter ao menos 3 caracteres. \nTente novamente.");
        }

        public static void IncorrectPassword()
        {
            Title.Error();
            Console.WriteLine("\nSenha incorreta. Tente novamente.");
        }

        public static void IncorrectUser()
        {
            Title.Error();
            Console.WriteLine("\nUsuário não existe. Tente novamente.");
        }

        public static void NoUserWithThatLogin()
        {
            Title.Error();
            Console.WriteLine("Não há funcionário com esse login. Tente novamente.");
        }

        public static void PressAnyKeyToReturn()
        {
            Console.WriteLine("Pressione qualquer tecla para retornar.");
        }

        public static void Returning()
        {
            Title.Returning();
        }
    }
}
