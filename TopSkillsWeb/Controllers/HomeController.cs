using Core;
using Core.Account;
using Data.Repository;
using Data.Services;
using Data.WebUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
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
        private readonly AttendanceService _attendance;
        private readonly WebUserService _webUser;

        public HomeController(ILogger<HomeController> logger, IRepository<User> us, StudentService _student, AttendanceService _attendance, WebUserService webUser)
        {
            _logger = logger;
            this.us = us;
            this._student = _student;
            this._attendance = _attendance;
            _webUser = webUser;
        }
        public async Task<IActionResult> Index()
        {
            var list = us.GetAll();

            var students = _student.GetAllStudentsAsync().Result;
            var studentsGroups = students.GroupBy(x => x.DateCreate);

            var Attendance = _attendance.GetAllAttendance().Result;
            var AttendanceGroups = Attendance.GroupBy(x => x.DateVisiting).Where(x=>x.Key != null);



            ChartModel chartStudent = new();
            chartStudent.chartElems = studentsGroups.Select(x => new ChartElem { Time = x.Key, Total = x.Count() }).ToList();
            if (!await _webUser.HasAccess(User.Identity.Name, "read", "Student"))
            {
                chartStudent.chartElems = new();
            }


            ChartModel chartAttendance = new();
            chartAttendance.chartElems = AttendanceGroups.Select(x => new ChartElem { Time = x.Key, Total = x.Count() }).ToList();
            if (!await _webUser.HasAccess(User.Identity.Name, "read", "Attendance"))
            {
                chartAttendance.chartElems = new();
            }

            ViewBag.StudentChart = JsonConvert.SerializeObject(chartStudent);
            ViewBag.AttendanceChart = JsonConvert.SerializeObject(chartAttendance);
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

        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
        public async Task<IActionResult> NotFound()
        {
            return View();
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
