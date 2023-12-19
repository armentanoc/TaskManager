using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.People
{
    internal class TechLeader : User
    {
        public TechLeader(string newName, string newLogin, string newPassword, string? newEmail = null) : base(newName, newLogin, newPassword, newEmail)
        {
            SetJob(JobEnum.TechLeader);
        }
    }
}
