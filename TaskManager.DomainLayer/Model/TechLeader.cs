
using TaskManager.ConsoleInteraction.Components;

namespace TaskManager.DomainLayer.Model
{
    internal class TechLeader : User
    {
        public TechLeader(string newName, string newLogin, string newPassword, string? newEmail = null) : base(newName, newLogin, newPassword, newEmail)
        {
            SetJob(JobEnum.TechLeader);
        }

        public override void Greeting()
        {
            Console.WriteLine(Title.GreetingTechLead());
        }
    }
}
