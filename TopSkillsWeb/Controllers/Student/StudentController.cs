using Microsoft.AspNetCore.Mvc;

namespace TopSkillsWeb.Controllers.Student
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
