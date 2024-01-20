using Core;
using Data.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using TopSkillsWeb.Models;

namespace TopSkillsWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<User> us;

        public HomeController(ILogger<HomeController> logger, IRepository<User> us)
        {
            _logger = logger;
            this.us = us;
        }

        public IActionResult Index()
        {
            var list = us.GetAll();
            return View(list);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
