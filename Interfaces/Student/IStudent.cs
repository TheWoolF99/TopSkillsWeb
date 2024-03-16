using Core.Abonement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Student
{
    public interface IStudent
    {
        Task<IEnumerable<Core.Student>> GetAllStudentsAsync();
        Task<Core.Student> GetStudentAsync(int id);
        Task<int> AddStudentAsync(Core.Student student);
        Task UpdateStudentAsync(Core.Student student);
        Task DeleteAsync(int id);
        void UpdateRange(List<Core.Student> student);
        
    }
}
