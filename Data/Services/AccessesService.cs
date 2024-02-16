using Core.Accesses;
using Core.Account;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class AccessesService
    {
        private readonly IAccessesRepository accesses;

        public AccessesService(IAccessesRepository accesses)
        {
            this.accesses = accesses;
        }


        public async Task<List<UserPermissions>> GetPermissions()
        {
            return await accesses.GetPermissions();
        }
        public async Task<List<RoleElement>> GetRoles(int PermissionId)
        {
            return await accesses.GetRoles(PermissionId);
        }
        public async Task<UserPermissions> GetPermission(int PermissionId)
        {
            return await accesses.GetPermission(PermissionId);
        }
        public async Task CreatePermission(UserPermissions model)
        {
            await accesses.CreatePermission(model);
        }
        public async Task<bool> IsPermissionExists(UserPermissions model)
        {
            return await accesses.IsPermissionExists(model);
        }
        public async Task TogglePermissionAccessType(int PermissionID, int RoleID, int AccessTypeID)
        {
            await accesses.TogglePermissionAccessType(PermissionID, RoleID, AccessTypeID);
        }
        public async Task<List<UserRole>> GetGlobalRolesList(bool OwnerApp)
        {
            return await accesses.GetGlobalRolesList(OwnerApp);
        }




    }
}
