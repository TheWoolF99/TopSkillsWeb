using Core;
using Interfaces.Course;
using Interfaces.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class CourseService
    {
        private readonly ICourse _course;

        public CourseService(ICourse course) => this._course = course;

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _course.GetAllCoursesAsync();
        }

        public async Task<Course> GetCourseAsync(int id)
        {
            return await _course.GetCourseAsync(id);
        }

        public async Task AddCourseAsync(Course course)
        {
            try
            {
                await _course.AddCourseAsync(course);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex); 
            }
        }

        public async Task UpdateCourseAsync(Course course)
        {
            await _course.UpdateCourseAsync(course);
        }

    }
}
