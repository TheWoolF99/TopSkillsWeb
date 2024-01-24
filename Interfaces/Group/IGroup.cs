using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace Interfaces.Group
{
    public interface IGroup
    {
        Task<IEnumerable<Core.Group>> GetAllGroupsAsync();
        Task<Core.Group> GetGroupAsync(int id);
        Task AddGroupAsync(Core.Group group);

    }
}
