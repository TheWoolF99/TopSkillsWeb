using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Pqc.Crypto.Lms;
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

        public CourseController(CourseService course, TeacherService _tS)
        {
            this._course = course;
            this._tS = _tS;
        }


        public async Task<IActionResult> Index()
        {
            var Courses = await _course.GetAllCoursesAsync();
            return View(Courses);
        }

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

        public async Task<IActionResult>  OnUpdateTableRows()
        {
            var Courses = await _course.GetAllCoursesAsync();
            return PartialView("RowsPart", Courses);
        }


    }
}
