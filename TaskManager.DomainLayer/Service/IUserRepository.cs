using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DomainLayer.Model.People;

namespace TaskManager.DomainLayer.Service
{
    public interface IUserRepository
    {
        User GetUserByLogin(string login);
    }
}
