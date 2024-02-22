using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopSkillsWeb.Attributes;
using TopSkillsWeb.Resources;
using TeacherModel = Core.Teacher;

namespace TopSkillsWeb.Controllers.Teacher
{
    [Authorize]
    public class TeacherController : Controller
    {
        /// <summary>
        /// Сервис для работы с курсами
        /// </summary>
        private readonly CourseService _course;
        /// <summary>
        /// Сервис для работы с преподавателями
        /// </summary>
        private readonly TeacherService _teacher;

        public TeacherController(CourseService _course, TeacherService _tS)
        {
            this._course = _course;
            this._teacher = _tS;
        }

        [HasAccess("Teacher", "read")]
        public async Task<IActionResult> Index()
        {
            return View(await _teacher.GetAllTeachersAsync());
        }

        [HasAccess("Teacher", "create")]
        public async Task<IActionResult> GetModalAddEditTeacher(int? TeacherId = null)
        {
            TeacherModel course = new();
            ViewBag.Title = Resource.CreateTeacher;
            if (TeacherId != null)
            {
                ViewBag.Title = Resource.EditCourse;
                course = await _teacher.GetTeacherAsync((int)TeacherId);
            }
            return PartialView("ModalNewTeacher", course);
        }

        [HasAccess("Teacher", "create")]
        public async Task<IActionResult> OnAddUpdateTeacher(TeacherModel Teacher)
        {
            bool add = Teacher.TeacherId == 0;
            if (add)
            {
                await _teacher.AddTeacherAsync(Teacher);
            }
            else
            {
                await _teacher.UpdateTeacherAsync(Teacher);
            }
            return RedirectToAction("ShowModalSuccess", "Home", new { message = add? Resource.TeacherAddDone : Resource.TeacherEditDone });
        }

        [HasAccess("Teacher", "read")]
        public async Task<IActionResult> OnUpdateTableRows()
        {
            var Teachers = await _teacher.GetAllTeachersAsync();
            return PartialView("RowsPart", Teachers);
        }
    }
}
