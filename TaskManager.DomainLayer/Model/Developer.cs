
using TaskManager.DomainLayer.Service;

namespace TaskManager.DomainLayer.Model
{
    internal class Developer : User
    {
        private readonly DeveloperMenuService _developerMenuService;
        public Developer(string newName, string newLogin, string? newEmail = null) : base(newName, newLogin, newEmail)
        {
            SetJob(JobEnum.Developer);
            _developerMenuService = new DeveloperMenuService(this);
        }

        public override void Greeting()
        {
            _developerMenuService.ShowMainMenu();
        }
    }
}
