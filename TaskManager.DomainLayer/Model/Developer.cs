using TaskManager.ConsoleInteraction.Components;

namespace TaskManager.DomainLayer.Model
{
    internal class Developer : User
    {
        public Developer(string newName, string newLogin, string? newEmail = null) : base(newName, newLogin, newEmail)
        {
            SetJob(JobEnum.Developer);
        }

        public override void Greeting()
        {
            Title.GreetingDev();
            //MenuOpcoesDev();
        }
    }
}
