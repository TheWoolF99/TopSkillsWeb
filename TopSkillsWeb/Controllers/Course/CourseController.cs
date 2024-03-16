using Data.Services;
using Data.WebUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using TopSkillsWeb.Attributes;
using TopSkillsWeb.Resources;
using CourseModel = Core.Course;

namespace TopSkillsWeb.Controllers.Course
{
    [Authorize]
    public class CourseController : Controller
    {
        /// <summary>
        /// Сервис для работы с курсами
        /// </summary>
        private readonly CourseService _course;
        /// <summary>
        /// Сервис для работы с преподавателями
        /// </summary>
        private readonly TeacherService _tS;
        private readonly WebUserService _webUser;

        public CourseController(CourseService course, TeacherService _tS, WebUserService webUser)
        {
            this._course = course;
            this._tS = _tS;
            _webUser = webUser;

        }

        [HasAccess("Course", "read")]
        public async Task<IActionResult> Index()
        {
            string name = User.Identity.Name;
            ViewBag.Edit = await _webUser.HasAccess(name, "edit", "Course");
            ViewBag.Delete = await _webUser.HasAccess(name, "delete", "Course");

            var Courses = await _course.GetAllCoursesAsync();
            return View(Courses);
        }

        [HasAccess("Course", "create")]
        public async Task<IActionResult> GetModalAddEditCourse(int? CourseId = null)
        {
            ViewBag.TeacherList = new SelectList(await _tS.GetAllTeachersAsync(), "TeacherId", "FullName");

            CourseModel course = new();
            ViewBag.Title = Resource.CreateCourse;
            if (CourseId != null)
            {
                ViewBag.Title = Resource.EditCourse;
                course = await _course.GetCourseAsync((int)CourseId);
            }
            return PartialView("ModalNewCourse", course);
        }

        [HasAccess("Course", "create")]
        public async Task<IActionResult> OnAddUpdateCourse(CourseModel Course)
        {
            if (Course.CourseId == 0)
            {
                await _course.AddCourseAsync(Course);
            }
            else
            {
                await _course.UpdateCourseAsync(Course);
            }
            return RedirectToAction("ShowModalSuccess", "Home", new { message = Course.CourseId == 0? Resource.CourseAddDone : Resource.CourseEditDone });
        }


        [HasAccess("Course", "read")]
        public async Task<IActionResult>  OnUpdateTableRows()
        {
            string name = User.Identity.Name;
            ViewBag.Edit = await _webUser.HasAccess(name, "edit", "Course");
            ViewBag.Delete = await _webUser.HasAccess(name, "delete", "Course");

            var Courses = await _course.GetAllCoursesAsync();
            return PartialView("RowsPart", Courses);
        }


        [HasAccess("Course", "delete")]
        public async Task<IActionResult> ConfirmDeleteCourse(int CoursetId)
        {
            return PartialView("ConfirmDelete", CoursetId);
        }

        [HasAccess("Course", "delete")]
        public async Task<IActionResult> OnDeleteCourse(int CoursetId)
        {
            await _course.DeleteAsync(CoursetId);
            return new EmptyResult();
        }

    }
}
