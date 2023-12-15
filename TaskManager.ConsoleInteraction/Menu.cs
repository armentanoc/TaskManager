
namespace ConsoleInteraction.ClassLib
{
    public class Menu
    {
        private readonly string[] items;
        private int selectedIndex;

        public Menu(string[] menuItems)
        {
            items = menuItems;
            selectedIndex = 0;
        }

        public int ShowMenu(string? title = null)
        {
            ConsoleKeyInfo key;
            Console.CursorVisible = false;

            try
            {
                do
                {
                    Console.Clear();
                    RenderMenu(title);

                    key = Console.ReadKey(true);

                    HandleKeyPress(key);

                } while (key.Key != ConsoleKey.Enter);
            }
            finally
            {
                Console.CursorVisible = true;
            }

            Console.WriteLine($"\nOpção selecionada: {items[selectedIndex]}\n");
            return selectedIndex;
        }

        private void RenderMenu(string? title = null)
        {
            if (title != null)
            {
                Console.WriteLine(title);
            }

            Console.WriteLine("\nSelecione uma opção: \n");

            for (int i = 0; i < items.Length; i++)
            {
                Console.ForegroundColor = (i == selectedIndex) ? ConsoleColor.Black : ConsoleColor.Gray;
                Console.BackgroundColor = (i == selectedIndex) ? ConsoleColor.Gray : ConsoleColor.Black;

                Console.WriteLine($"{items[i]}");

                Console.ResetColor();
            }

            Console.WriteLine("\nAperte enter para confirmar.");
        }

        private void HandleKeyPress(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = Math.Max(0, selectedIndex - 1);
                    break;

                case ConsoleKey.DownArrow:
                    selectedIndex = Math.Min(items.Length - 1, selectedIndex + 1);
                    break;
            }
        }
    }
}