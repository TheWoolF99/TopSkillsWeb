using Core;
using Interfaces.Teacher;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class TeacherRepository : ITeacher
    {
        private DbContextFactory _context;

        public TeacherRepository(DbContextFactory context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Teacher>> GetAllTeachersAsync()
        {
            var db = _context.Create(typeof(TeacherRepository));
            return await db.Teachers.ToListAsync();
        }


        public async Task<Teacher> GetTeacherAsync(int id)
        {
            var db = _context.Create(typeof(TeacherRepository));
            return await db.Teachers.SingleAsync(x => x.TeacherId == id);
        }


        public async Task AddTeacherAsync(Teacher teacher)
        {
            var db = _context.Create(typeof(TeacherRepository));
            await db.Teachers.AddAsync(teacher);
            await db.SaveChangesAsync();
        }

        public async Task UpdateTeacherAsync(Teacher teacher)
        {
            var db = _context.Create(typeof(TeacherRepository));
            var dbt = db.Teachers.FirstOrDefault(x => x.TeacherId == teacher.TeacherId);
            teacher.UpdateFieldTo(dbt);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var db = _context.Create(typeof(TeacherRepository));

            await db.SaveChangesAsync();
        }
    }
}
