using TaskManager.DomainLayer.Service.CustomMenu;

namespace TaskManager.DomainLayer.Model.People
{
    public class Developer : User
    {
        private readonly DeveloperMenu _developerMenuService;

        public Developer(string newName, string newLogin, string? newEmail = null) : base(newName, newLogin, newEmail)
        {
            SetJob(JobEnum.Developer);
            _developerMenuService = new DeveloperMenu(this);
        }

        public Developer(string id, string? name, string? login, string? password, string? email, JobEnum job) : base(id, name, login, password, email, job)
        {
            SetJob(JobEnum.Developer);
            _developerMenuService = new DeveloperMenu(this);
        }
        public override void Greeting()
        {
            _developerMenuService.ShowMainMenu();
        }
    }
}
