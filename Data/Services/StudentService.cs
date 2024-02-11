using Core;
using Core.Abonement;
using Interfaces.Photo;
using Interfaces.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class StudentService
    {
        private readonly IStudent _student;

        public StudentService(IStudent student) => this._student = student;

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _student.GetAllStudentsAsync();
        }

        public async Task<Student> GetStudentAsync(int id)
        {
            return await _student.GetStudentAsync(id);
        }

        public async Task AddStudentAsync(Student Student)
        {
            await _student.AddStudentAsync(Student);
        }

        public async Task UpdateStudentAsync(Core.Student student)
        {
            await _student.UpdateStudentAsync(student);
        }

        public void UpdateRange(List<Core.Student> student)
        {
            _student.UpdateRange(student);
        }

        

    }
}
