using TaskManager.DomainLayer.Service;

namespace TaskManager.DomainLayer.Model.People
{
    public class TechLeader : User
    {
        private readonly TechLeaderMenuService _techLeaderMenuService;
        public TechLeader(string newName, string newLogin, string? newEmail = null) : base(newName, newLogin, newEmail)
        {
            SetJob(JobEnum.TechLeader);
            _techLeaderMenuService = new TechLeaderMenuService(this);
        }

        public override void Greeting()
        {
            _techLeaderMenuService.ShowMainMenu();
        }
    }
}
