using Core;
using Core.Abonement;
using Data.Services;
using Data.WebUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopSkillsWeb.Attributes;
using TopSkillsWeb.Resources;
using StudentModel = Core.Student;

namespace TopSkillsWeb.Controllers.Student
{
    [Authorize]
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
        private readonly AbonementService _abonement;
        private readonly WebUserService _webUser;
        public StudentController(StudentService _sS, TeacherService _tS, AbonementService abonement, WebUserService _webUser)
        {
            this._student = _sS;
            this._teacher = _tS;
            this._abonement = abonement;
            this._webUser = _webUser;
        }

        [HasAccess("Students","read")]
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
                ViewBag.Title = Resource.EditStudent;
                student = await _student.GetStudentAsync((int)StudentId);
            }
            return PartialView("ModalNewStudent", student);
        }
        public async Task<IActionResult> OnAddUpdateStudent(StudentModel Student)
        {
            bool add = Student.StudentId == 0;
            if (add)
            {
                int newStudentId = await _student.AddStudentAsync(Student);
                var Ab = new Abonement();
                Ab.StudentId = newStudentId;
                await _abonement.AddNewAbonement(Ab);
            }
            else
            {
                await _student.UpdateStudentAsync(Student);

            }
            return RedirectToAction("ShowModalSuccess", "Home", new { message = add? Resource.StudentAddDone : Resource.StudentEditDone });
        }


        public async Task<IActionResult> OnUpdateTableRows()
        {
            var Students = await _student.GetAllStudentsAsync();
            return PartialView("RowsPart", Students);
        }


        public async Task<IActionResult> OnRefreshAbonement(int StudentId)
        {
            await _abonement.RefreshAbonement(StudentId);
            return new EmptyResult();
        }


    }
}
