using Core.Accesses;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Account
{
    public class UserRolePermissions
    {
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string PermissionName { get; set; }
        public string AccessTypeName { get; set; }
    }
}
