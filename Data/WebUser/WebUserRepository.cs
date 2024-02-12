﻿using Core.Accesses;
using Core.Account;
using Microsoft.AspNetCore.Identity;
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
    }
}
