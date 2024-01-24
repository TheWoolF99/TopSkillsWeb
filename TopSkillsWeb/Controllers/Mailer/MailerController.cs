using Microsoft.AspNetCore.Mvc;

namespace TopSkillsWeb.Controllers.Mailer
{
    public class MailerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public async Task GetExpiredSubscription()
        {
            
        }

    }
}
