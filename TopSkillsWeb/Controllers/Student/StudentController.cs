using Core;
using Data.Services;
using Microsoft.AspNetCore.Mvc;
using TopSkillsWeb.Resources;
using StudentModel = Core.Student;

namespace TopSkillsWeb.Controllers.Student
{
    public class StudentController : Controller
    {
        /// <summary>
        /// Сервис для работы с преподавателями
        /// </summary>
        private readonly TeacherService _teacher;
        /// <summary>
        /// Сервис для работы с преподавателями
        /// </summary>
        private readonly StudentService _student;
        public StudentController(StudentService _sS, TeacherService _tS)
        {
            this._student = _sS;
            this._teacher = _tS;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _student.GetAllStudentsAsync());
        }


        public async Task<IActionResult> GetModalAddEditStudent(int? StudentId = null)
        {
            StudentModel student = new();
            ViewBag.Title = Resource.CreateStudent;
            if (StudentId != null)
            {
                ViewBag.Title = Resource.EditCourse;
                student = await _student.GetStudentAsync((int)StudentId);
            }
            return PartialView("ModalNewStudent", student);
        }
        public async Task<IActionResult> OnAddUpdateStudent(StudentModel Student)
        {
            if (Student.StudentId == 0)
            {
                await _student.AddStudentAsync(Student);
            }
            else
            {
                await _student.UpdateStudentAsync(Student);
            }
            return RedirectToAction("ShowModalSuccess", "Home", new { message = Resource.StudentAddDone });
        }

        public async Task<IActionResult> OnUpdateTableRows()
        {
            var Students = await _student.GetAllStudentsAsync();
            return PartialView("RowsPart", Students);
        }
        
    }
}
