using Core;
using Data.Repository;
using Interfaces.Group;
using Interfaces.Photo;
using Microsoft.EntityFrameworkCore;
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

        public async Task<int> AddGroupAsync(Group group)
        {
            return await _group.AddGroupAsync(group);
        }

        public async Task Update(Group group)
        {
            await _group.Update(group);
        }

        public async Task AddGroupStudentsAsync(IEnumerable<GroupStudent> groupStudents)
        {
            await _group.AddGroupStudentsAsync(groupStudents);
        }

        public async Task UpdateGroupWithStudents(int groupId, List<int> StudentsIds)
        {
            await _group.UpdateGroupWithStudents(groupId, StudentsIds);
        }
    }

    
}
