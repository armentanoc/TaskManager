using TaskManager.DomainLayer.Model.People;

namespace TaskManager.DomainLayer.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        User GetUserByLogin(string login);
    }
}
