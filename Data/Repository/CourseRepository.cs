using Core;
using Interfaces.Course;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class CourseRepository : ICourse
    {
        private DbContextFactory _context;

        public CourseRepository(DbContextFactory context)
        {
            this._context = context;
        }


        public async Task<IEnumerable<Course>> GetAllCourseAsync()
        {
            var db = _context.Create(typeof(CourseRepository));
            return await db.Courses.ToListAsync();
        }


        public async Task<Course> GetCourseAsync(int id)
        {
            var db = _context.Create(typeof(CourseRepository));
            return await db.Courses.SingleAsync(x => x.CourseId == id);
        }


        public async Task AddCourseAsync(Course course)
        {
            var db = _context.Create(typeof(CourseRepository));
            await db.Courses.AddAsync(course);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var db = _context.Create(typeof(CourseRepository));

            await db.SaveChangesAsync();
        }
    }
}
