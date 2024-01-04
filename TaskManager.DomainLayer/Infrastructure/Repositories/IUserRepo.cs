using TaskManager.DomainLayer.Model.People;

namespace TaskManager.DomainLayer.Infrastructure.Repositories
{
    public interface IUserRepo
    {
        User GetUserByLogin(string login);
    }
}
