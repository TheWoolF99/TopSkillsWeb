using Core;
using Core.Account;
using Data.Repository;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

using TopSkillsWeb.Models;

namespace TopSkillsWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<User> us;
        private readonly StudentService _student;

        public HomeController(ILogger<HomeController> logger, IRepository<User> us, StudentService _student)
        {
            _logger = logger;
            this.us = us;
            this._student = _student;
        }
        public IActionResult Index()
        {
            var list = us.GetAll();

            var students = _student.GetAllStudentsAsync().Result;
            var studentsGroups = students.GroupBy(x => x.DateCreate);
            ChartModel chart = new();
            chart.DateStringArray = studentsGroups.Select(x => x.Key.ToString("yyy-MM-dd")).ToArray();
            chart.DataArray = studentsGroups.Select(x => x.Count()).ToArray();
            ViewBag.StudentChart = JsonConvert.SerializeObject(chart);
            ViewBag.TotalStudents = students.Count();
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
        public async Task<IActionResult> ShowModalError(string message)
        {
            return PartialView("ModalError", message);
        }
        public async Task<IActionResult> ShowModalSuccess(string message)
        {
            return PartialView("ModalSuccess", message);
        }
        public async Task<IActionResult> ShowSpinner()
        {
            return PartialView("ModalSpinner");
        }
    }
}
