using Core;
using Interfaces.Group;
using Interfaces.Photo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class GroupService
    {
        private readonly IGroup _group;

        public GroupService(IGroup group) => this._group = group;

        public async Task<IEnumerable<Group>> GetAllGroupsAsync()
        {
            return await _group.GetAllGroupsAsync();
        }

        public async Task<Group> GetGroupAsync(int id)
        {
            return await _group.GetGroupAsync(id);
        }

        public async Task AddGroupAsync(Group group)
        {
            await _group.AddGroupAsync(group);
        }
    }
}
