using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Teacher
{
    public interface ITeacher
    {
        Task<IEnumerable<Core.Teacher>> GetAllTeachersAsync();
        Task<Core.Teacher> GetTeacherAsync(int id);
        Task AddTeacherAsync(Core.Teacher teacher);
        Task UpdateTeacherAsync(Core.Teacher teacher);
    }
}
