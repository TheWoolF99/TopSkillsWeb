using Microsoft.AspNetCore.Mvc;

namespace TopSkillsWeb.Controllers.Course
{
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
