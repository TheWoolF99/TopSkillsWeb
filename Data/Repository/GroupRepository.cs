using Core;
using Interfaces.Group;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class GroupRepository : IGroup
    {
        private DbContextFactory _context;

        public GroupRepository(DbContextFactory context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Group>> GetAllGroupsAsync()
        {
            var db = _context.Create(typeof(GroupRepository));
            return await db.Groups.ToListAsync();
        }


        public async Task<Group> GetGroupAsync(int id)
        {
            var db = _context.Create(typeof(GroupRepository));
            return await db.Groups.SingleAsync(x => x.GroupId == id);
        }


        public async Task AddGroupAsync(Group group)
        {
            var db = _context.Create(typeof(GroupRepository));
            await db.Groups.AddAsync(group);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var db = _context.Create(typeof(GroupRepository));
            
            await db.SaveChangesAsync();
        }
    }
}
