using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Course
{
    public interface ICourse 
    {
        Task<IEnumerable<Core.Course>> GetAllCoursesAsync();
        Task<Core.Course> GetCourseAsync(int id);
        Task AddCourseAsync(Core.Course course);
        Task UpdateCourseAsync(Core.Course course);

    }
}
