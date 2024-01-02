using TaskManager.DomainLayer.Model.People;

namespace TaskManager.DomainLayer.Repositories
{
    public interface IUserRepository
    {
        User GetUserByLogin(string login);
    }
}
