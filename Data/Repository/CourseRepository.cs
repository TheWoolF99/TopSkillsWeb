using Core;
using Interfaces.Course;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Data.Repository
{
    public class CourseRepository : ICourse
    {
        private DbContextFactory _context;

        public CourseRepository(DbContextFactory context)
        {
            this._context = context;
        }


        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            var db = _context.Create(typeof(CourseRepository));
            return await db.Courses.ToListAsync();
        }


        public async Task<Course> GetCourseAsync(int id)
        {
            var db = _context.Create(typeof(CourseRepository));
            //.Include(x=>x.Teacher)
            return await db.Courses.FirstOrDefaultAsync(x => x.CourseId == id);
        }


        public async Task AddCourseAsync(Course course)
        {
            var db = _context.Create(typeof(CourseRepository));
            //if(course.Teacher!=null)
            //    course.Teacher = await db.Teachers.FirstOrDefaultAsync(x => x.TeacherId == course.Teacher.TeacherId);
            await db.Courses.AddAsync(course);
            await db.SaveChangesAsync();
        }

        public async Task UpdateCourseAsync(Course course)
        {
            var db = _context.Create(typeof(CourseRepository));
            //if (course.Teacher != null)
            //    course.Teacher = await db.Teachers.FirstOrDefaultAsync(x => x.TeacherId == course.Teacher.TeacherId);
            db.Courses.Update(course);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var db = _context.Create(typeof(CourseRepository));
            await db.Courses.Where(x => x.CourseId == id).DeleteAsync();
            await db.SaveChangesAsync();
        }
    }
}
