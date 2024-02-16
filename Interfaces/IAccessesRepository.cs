using Core.Accesses;
using Core.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IAccessesRepository
    {
        Task<List<UserPermissions>> GetPermissions();
        Task<List<RoleElement>> GetRoles(int PermissionId);
        Task<UserPermissions> GetPermission(int PermissionId);
        Task CreatePermission(UserPermissions model);
        Task<bool> IsPermissionExists(UserPermissions model);
        Task TogglePermissionAccessType(int PermissionID, int RoleID, int AccessTypeID);
        Task<List<UserRole>> GetGlobalRolesList(bool OwnerApp);
    }
}
