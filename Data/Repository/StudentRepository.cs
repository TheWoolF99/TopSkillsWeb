using Core;
using Interfaces.Student;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class StudentRepository : IStudent
    {
        private DbContextFactory _context;

        public StudentRepository(DbContextFactory context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            var db = _context.Create(typeof(StudentRepository));
            return await db.Students.ToListAsync();
        }


        public async Task<Student> GetStudentAsync(int id)
        {
            var db = _context.Create(typeof(StudentRepository));
            return await db.Students.SingleAsync(x => x.StudentId == id);
        }


        public async Task AddStudentsAsync(Student student)
        {
            var db = _context.Create(typeof(StudentRepository));
            await db.Students.AddAsync(student);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var db = _context.Create(typeof(StudentRepository));

            await db.SaveChangesAsync();
        }
    }
}
