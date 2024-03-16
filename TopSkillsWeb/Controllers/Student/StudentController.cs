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
            string name = User.Identity.Name;
            ViewBag.Edit = await _webUser.HasAccess(name, "edit", "Students");
            ViewBag.Delete = await _webUser.HasAccess(name, "delete", "Students");

            return View(await _student.GetAllStudentsAsync());
        }

        [HasAccess("Students", "create")]
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
        [HasAccess("Students", "create")]
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

        [HasAccess("Students", "read")]
        public async Task<IActionResult> OnUpdateTableRows()
        {
            string name = User.Identity.Name;
            ViewBag.Edit = await _webUser.HasAccess(name, "edit", "Students");
            ViewBag.Delete = await _webUser.HasAccess(name, "delete", "Students");
            var Students = await _student.GetAllStudentsAsync();
            return PartialView("RowsPart", Students);
        }

        [HasAccess("Students", "delete")]
        public async Task<IActionResult> ConfirmDeleteStudent(int StudentId)
        {
            return PartialView("ConfirmDelete", StudentId);
        }

        [HasAccess("Students", "delete")]
        public async Task<IActionResult> OnDeleteStudent(int StudentId)
        {
            await _student.DeleteAsync(StudentId);
            return new EmptyResult();
        }


        [HasAccess("RefreshAbonement", "edit")]
        public async Task<IActionResult> OnRefreshAbonement(int StudentId)
        {
            await _abonement.RefreshAbonement(StudentId);
            return new EmptyResult();
        }


    }
}
