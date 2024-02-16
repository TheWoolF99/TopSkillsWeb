using Core.Accesses;
using Core.Account;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class AccessesRepository : IAccessesRepository
    {
        private readonly DbContextFactory _context;

        public AccessesRepository(DbContextFactory dbContextFactory)
        {
            this._context = dbContextFactory;
        }


        public async Task<List<UserPermissions>> GetPermissions()
        {
            var db = _context.Create(typeof(AccessesRepository));
            return await db.Permissions.OrderBy(x => x.Description).ToListAsync();
        }

        public async Task<UserPermissions> GetPermission(int PermissionId)
        {
            var db = _context.Create(typeof(AccessesRepository));
            return await db.Permissions.Where(x => x.PermissionId == PermissionId).FirstOrDefaultAsync();
        }

        public async Task<List<UserRole>> GetGlobalRolesList(bool OwnerApp)
        {
            var db = _context.Create(typeof(AccessesRepository));
            var model = await db.AspNetRoles.OrderBy(x => x.Name).ToListAsync();
            if (!OwnerApp)
            {
                model.Remove(model.Where(x => x.Name.ToLower() == "ownerapp").First());
            }
            return model;
        }

        public async Task<List<RoleElement>> GetRoles(int PermissionId)
        {
            var db = _context.Create(typeof(AccessesRepository));
            var identityRoles = db.AspNetRoles.OrderBy(x => x.Name).ToList();
            List<RoleElement> result = new List<RoleElement>();
            foreach (var item in identityRoles)
            {
                RoleElement roleitem = new();
                roleitem.Id = item.Id;
                roleitem.Name = item.Name;
                roleitem.Description = item.Description;
                List<RolePermissions> rolePermissions = db.RolePermissions.Where(x => x.PermisionId == PermissionId && x.RoleId == Convert.ToInt32(item.Id)).ToList();
                roleitem.AllAccess = rolePermissions.Where(x => x.AccessTypeId == 1).FirstOrDefault() != null ? true : false;
                if (roleitem.AllAccess)
                {
                    roleitem.Read = true;
                    roleitem.Add = true;
                    roleitem.Edit = true;
                    roleitem.Delete = true;
                }
                else
                {
                    roleitem.Read = rolePermissions.Where(x => x.AccessTypeId == 2).FirstOrDefault() != null ? true : false;
                    roleitem.Add = rolePermissions.Where(x => x.AccessTypeId == 3).FirstOrDefault() != null ? true : false;
                    roleitem.Edit = rolePermissions.Where(x => x.AccessTypeId == 4).FirstOrDefault() != null ? true : false;
                    roleitem.Delete = rolePermissions.Where(x => x.AccessTypeId == 5).FirstOrDefault() != null ? true : false;
                }
                result.Add(roleitem);
            }
            return result;
        }
        public async Task CreatePermission(UserPermissions model)
        {
            var db = _context.Create(typeof(AccessesRepository));
            db.Permissions.Add(model);
            await db.SaveChangesAsync();
            //OwnerApp всегда полные права
            await TogglePermissionAccessType(model.PermissionId, 1, 1);

        }
        public async Task<bool> IsPermissionExists(UserPermissions model)
        {
            bool result = false;
            var db = _context.Create(typeof(AccessesRepository));
            result = db.Permissions.Where(x => x.Name == model.Name || x.Description == model.Description).FirstOrDefault() == null ? false : true;
            return result;

        }
        public async Task TogglePermissionAccessType(int PermissionID, int RoleID, int AccessTypeID)
        {
            var db = _context.Create(typeof(AccessesRepository));
            if (AccessTypeID == 1)
            {
                var model = db.RolePermissions.Where(x => x.PermisionId == PermissionID && x.AccessTypeId == 1 && x.RoleId == RoleID).FirstOrDefault();
                if (model == null)
                {
                    List<RolePermissions> rolePermissions = new();
                    for (int i = 1; i < 6; i++)
                    {
                        RolePermissions item = new RolePermissions();
                        item.PermisionId = PermissionID;
                        item.RoleId = RoleID;
                        item.AccessTypeId = i;
                        rolePermissions.Add(item);
                    }
                    await db.RolePermissions.AddRangeAsync(rolePermissions);
                    await db.SaveChangesAsync();
                }
                else
                {
                    List<RolePermissions> rolePermissions = db.RolePermissions.Where(x => x.PermisionId == PermissionID && x.RoleId == RoleID).ToList();
                    db.RolePermissions.RemoveRange(rolePermissions);
                    await db.SaveChangesAsync();
                }
            }
            else
            {
                bool AllAccessEnabled = db.RolePermissions.Where(x => x.PermisionId == PermissionID && x.AccessTypeId == 1 && x.RoleId == RoleID).FirstOrDefault() != null ? true : false;
                if (!AllAccessEnabled)
                {
                    var model = db.RolePermissions.Where(x => x.PermisionId == PermissionID && x.AccessTypeId == AccessTypeID && x.RoleId == RoleID).FirstOrDefault();
                    if (model == null)
                    {
                        RolePermissions rolePermission = new();
                        rolePermission.PermisionId = PermissionID;
                        rolePermission.RoleId = RoleID;
                        rolePermission.AccessTypeId = AccessTypeID;
                        await db.RolePermissions.AddAsync(rolePermission);
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        db.RolePermissions.Remove(model);
                        await db.SaveChangesAsync();
                    }
                }
            }
        }

    }
}
