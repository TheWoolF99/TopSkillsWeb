using Core;
using AttendanceModel = Core.Attendance;

namespace Interfaces.Attendance
{
    public interface IAttendance
    {
        Task<IEnumerable<AttendanceModel>> GetAllAttendance();

        Task<IEnumerable<AttendanceModel>> GetAttendanceByGroupIdAndDate(int GroupId, DateTime Date);

        Task<IEnumerable<AttendanceModel>> GetAttendancesByDateRange(DateTime start, DateTime end);

        Task<IEnumerable<AttendanceModel>> GetAttendancesByDateRange(DateTime date);

        Task<bool> OnAddAttendanceByDateAndGroupId(AttendanceModel attendance);

        Task OnStartAttendance(List<AttendanceModel> attendances);

        Task OnDeleteAttendance(int GroupId, DateTime Date);

        Task<List<AttendanceDeductionLog>> GetStudentDeductionHistory(int? studentId);
    }
}