




namespace TaskManager.ConsoleInteraction.Components
{
    public class Message
    {
        public static void AuthenticationFailed()
        {

            Title.Error();
            Console.WriteLine("\nFalha na autenticação. Tente novamente.");
        }

        public static void IncorrectPassword()
        {
            Title.Error();
            Console.WriteLine("\nSenha incorreta. Tente novamente.");
            PressAnyKeyToReturn();
        }

        public static void IncorrectUser()
        {
            Title.Error();
            Console.WriteLine("\nUsuário não existe. Tente novamente.");
            PressAnyKeyToReturn();
        }

        public static void NoUserWithThatLogin()
        {
            Title.Error();
            Console.WriteLine("Não há funcionário com esse login. Tente novamente.");
            PressAnyKeyToReturn();
        }

        public static void PressAnyKeyToReturn()
        {
            Console.WriteLine("\nPressione qualquer tecla para retornar.");
        }

        public static void Returning()
        {
            Title.Returning();
            PressAnyKeyToReturn();
        }

        public static void SmallPassword()
        {
            Console.WriteLine("\nA senha deve ter pelo menos 3 caracteres.");
            PressAnyKeyToReturn();
        }
    }
}
