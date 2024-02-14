using Core.Accesses;
using Core.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Data.WebUser
{
    public class WebUserRepository : IWebUser
    {
        private readonly DbContextFactory dbContextFactory;
        private readonly UserManager<User> _userManager;
        public WebUserRepository(DbContextFactory dbContextFactory, UserManager<User> _userManager)
        {
            this.dbContextFactory = dbContextFactory;
            this._userManager = _userManager;
        }
        public async Task<bool> HasAccess(string UserName, string AccessType, string permissionName)
        {
            var dbContext = dbContextFactory.Create(typeof(WebUserRepository));
            var user = await _userManager.FindByNameAsync(UserName);
            
            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || roles.Count() == 0)
            {
                return false;
            }
            else
            {
                if(roles.Contains("OwnerApp"))
                    return true;
            }
            int RoleID = Convert.ToInt32(dbContext.AspNetRoles.Where(x => x.Name == roles[0]).FirstOrDefault().Id);
            int AccessTypeId = dbContext.AccessTypes.Where(x => x.Code == AccessType).FirstOrDefault().TypeId;
            UserPermissions? permission = dbContext.Permissions.Where(x => x.Name == permissionName).FirstOrDefault();
            if (permission == null)
            {
                return false;
            }
            int PermissionId = permission.PermissionId;
            var access = dbContext.RolePermissions.Where(x => x.PermisionId == PermissionId && x.RoleId == RoleID && x.AccessTypeId == AccessTypeId).FirstOrDefault();
            if (access == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<string> GetUserGuid(string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            return user.Id;
        }
        public async Task<string> GetUserRoles(string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            var roles = await _userManager.GetRolesAsync(user);
            return roles.First();
        }
        public async Task<bool> HasExtraAccess(string UserName)
        {
            var dbContext = dbContextFactory.Create(typeof(WebUserRepository));
            var user = await _userManager.FindByNameAsync(UserName);
            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || roles.Count() == 0)
            {
                return false;
            }
            if (roles[0] == "OwnerApp" || roles[0] == "SudoAdmin")
            {
                return true;
            }
            return false;
        }


        public async Task<IEnumerable<UserRolePermissions>> GetUserRolesPermissions(string UserName)
        {
            var dbContext = dbContextFactory.Create(typeof(WebUserRepository));

            var query = from u in dbContext.AspNetUsers
                        join ur in dbContext.AspNetUserRoles on u.Id equals ur.UserId into urGroup
                        from ur in urGroup.DefaultIfEmpty()
                        join r in dbContext.AspNetRoles on ur.RoleId equals r.Id into rGroup
                        from r in rGroup.DefaultIfEmpty()
                        join rp in dbContext.RolePermissions on  Convert.ToInt32(ur.RoleId)  equals rp.RoleId into rpGroup
                        from rp in rpGroup.DefaultIfEmpty()
                        join p in dbContext.Permissions on rp.PermisionId equals p.PermissionId into pGroup
                        from p in pGroup.DefaultIfEmpty()
                        join aty in dbContext.AccessTypes on rp.AccessTypeId equals aty.TypeId into atyGroup
                        from aty in atyGroup.DefaultIfEmpty()
                        where u.UserName == UserName
                        select new UserRolePermissions { UserName= u.UserName, RoleName = r.Name, PermissionName = p.Description, AccessTypeName = aty.Name };

            var list =  await query.ToListAsync();
            return list;
        }

    }
}
