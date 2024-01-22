using Core.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Users
{
    public interface IUserRepository
    {
        public List<User> GetAll();
    }
}
