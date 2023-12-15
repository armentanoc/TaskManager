namespace TaskManager.Core
{
    internal interface IConsole
    {
        public static void WaitThreeSeconds()
        {
            Console.CursorVisible = false;
            Thread.Sleep(3000);
        }
    }
}