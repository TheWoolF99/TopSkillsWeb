using Microsoft.AspNetCore.Mvc;

namespace TopSkillsWeb.Controllers.Teacher
{
    public class TeacherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
