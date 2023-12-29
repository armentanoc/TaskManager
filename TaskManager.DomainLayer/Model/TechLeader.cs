
using TaskManager.ConsoleInteraction.Components;

namespace TaskManager.DomainLayer.Model
{
    internal class TechLeader : User
    {
        public TechLeader(string newName, string newLogin, string? newEmail = null) : base(newName, newLogin, newEmail)
        {
            SetJob(JobEnum.TechLeader);
        }

        public override void Greeting()
        {
            Title.GreetingTechLead();
        }
    }
}
