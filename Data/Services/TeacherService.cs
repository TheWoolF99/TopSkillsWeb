using Core;
using Interfaces.Photo;
using Interfaces.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class TeacherService
    {
        private readonly ITeacher _teacher;

        public TeacherService(ITeacher teacher) => this._teacher = teacher;

        public async Task<IEnumerable<Teacher>> GetAllTeachersAsync()
        {
            return await _teacher.GetAllTeachersAsync();
        }

        public async Task<Teacher> GetTeacherAsync(int id)
        {
            return await _teacher.GetTeacherAsync(id);
        }

        public async Task AddTeacherAsync(Teacher Teacher)
        {
            await _teacher.AddTeacherAsync(Teacher);
        }

        public async Task UpdateTeacherAsync(Teacher teacher)
        {
            await _teacher.UpdateTeacherAsync(teacher);
        }

        public async Task DeleteAsync(int id)
        {
            await _teacher.DeleteAsync(id);
        }

    }
}
