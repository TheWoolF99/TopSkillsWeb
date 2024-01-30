using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceModel = Core.Attendance;
namespace Interfaces.Attendance
{
    public interface IAttendance
    {
        Task<AttendanceModel> GetAttendanceByGroupIdAndDate(int GroupId, DateTime Date);
        Task<IEnumerable<AttendanceModel>> GetAttendancesByDateRange(DateTime start, DateTime end);
        Task<bool> OnAddAttendanceByDateAndGroupId(AttendanceModel attendance);
    }
}
