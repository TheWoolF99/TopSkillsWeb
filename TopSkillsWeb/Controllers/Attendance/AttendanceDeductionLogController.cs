using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TopSkillsWeb.Controllers.Attendance
{
    [Authorize]
    public class AttendanceDeductionLogController : Controller
    {
        private readonly StudentService _studentService;
        private readonly AttendanceService _attendanceService;

        public AttendanceDeductionLogController(StudentService studentService,
            AttendanceService attendanceService)
        {
            _studentService = studentService;
            _attendanceService = attendanceService;
        }

        public async Task<IActionResult> Index(int? studentId)
        {
            var model = await _attendanceService.GetStudentDeductionHistory(studentId);
            var allStudents = await _studentService.GetAllStudentsAsync();
            ViewBag.StudentsSelectList = new SelectList(allStudents, "StudentId", "ShortName", studentId);
            return View(model);
        }
    }
}