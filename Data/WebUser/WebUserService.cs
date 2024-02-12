using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.WebUser
{
    public class WebUserService
    {
        private readonly IWebUser webUser;

        public WebUserService(IWebUser webUser) => this.webUser = webUser;

        public async Task<bool> HasAccess(string UserName, string AccessType, string Permission)
        {
            return await webUser.HasAccess(UserName, AccessType, Permission);
        }

        public async Task<string> GetUserGuid(string UserName)
        {
            return await webUser.GetUserGuid(UserName);
        }

        public async Task<string> GetUserRoles(string UserName)
        {
            return await webUser.GetUserRoles(UserName);
        }
        public async Task<bool> HasExtraAccess(string UserName)
        {
            return await webUser.HasExtraAccess(UserName);
        }
    }
}
