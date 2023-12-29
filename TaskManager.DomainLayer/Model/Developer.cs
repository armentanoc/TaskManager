using TaskManager.ConsoleInteraction.Components;

namespace TaskManager.DomainLayer.Model
{
    internal class Developer : User
    {
        public Developer(string newName, string newLogin, string newPassword, string? newEmail = null) : base(newName, newLogin, newPassword, newEmail)
        {
            SetJob(JobEnum.Developer);
        }

        public override void Greeting()
        {
            Console.WriteLine(Title.GreetingDev());
        }
    }
}
