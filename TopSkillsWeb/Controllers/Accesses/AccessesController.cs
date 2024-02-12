using Data.Services;
using Data.WebUser;
using Microsoft.AspNetCore.Mvc;

namespace TopSkillsWeb.Controllers.Accesses
{
    public class AccessesController : Controller
    {
        private readonly AccessesService _AccessesService;
        private readonly WebUserService _webUser;

        public AccessesController(AccessesService _AccessesService, WebUserService webUserService)
        {
            this._AccessesService = _AccessesService;
            this._webUser = webUserService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
