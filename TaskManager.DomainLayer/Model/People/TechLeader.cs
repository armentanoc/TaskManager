using TaskManager.DomainLayer.Service.CustomMenu;

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

        public TechLeader(string id, string? name, string? login, string? password, string? email, JobEnum job) : base(id, name, login, password, email, job)
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
