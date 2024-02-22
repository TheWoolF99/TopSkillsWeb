using Core.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.WebUser
{
    public interface IWebUser
    {
        public Task<bool> HasAccess(string UserName, string AccessType, string Permission);
        public Task<string> GetUserGuid(string UserName);
        public Task<string> GetUserRoles(string UserName);
        public Task<bool> HasExtraAccess(string UserName);
        public Task<IEnumerable<UserRolePermissions>> GetUserRolesPermissions(string UserName);
        public Task<IEnumerable<User>> GetAllUsers();
        public Task<IEnumerable<UserRole>> GetAllRoles();
    }
}
