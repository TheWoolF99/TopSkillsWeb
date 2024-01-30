using Microsoft.AspNetCore.Mvc;
using AttendanceModel = Core.Attendance;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Data.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopSkillsWeb.Resources;
using Newtonsoft.Json;

namespace TopSkillsWeb.Controllers.Attendance
{
    public class AttendanceController : Controller
    {
        /// <summary>
        /// Сервис для работы с группами
        /// </summary>
        private readonly GroupService _gS;
        private readonly AttendanceService _aS;

        public AttendanceController(GroupService groupService, AttendanceService attendanceService)
        {
            this._gS = groupService;
            this._aS = attendanceService;
        }

        public IActionResult Index()
        {
            return View();
        }



        public async Task<IActionResult> GetModalAddAttendance(string date)
        {
            
            ViewBag.GroupList = new SelectList(await _gS.GetAllGroupsAsync(), "GroupId", "Name");
            ViewBag.Date = date;
            ViewBag.Title = $"{Resource.AddAttendanceOn} {date}";
            return PartialView("AddPlanAttendance");
        }


        public async Task<IActionResult> CreateNewAttendance(AttendanceModel model)
        {
            var AddDone = await _aS.OnAddAttendanceByDateAndGroupId(model);
            return new EmptyResult();
        }



        public async Task<JsonResult> GetCalendarData(string CurrentYear, string CurrentMounth)
        {
            //List<AttendanceModel> lst = new();
            //var DateStart = DateTime.Parse($"{CurrentYear}.{CurrentMounth}.01");

            //for (int i = 0; i < 31; i++)
            //{
            //    for (int j = 0; j < new Random().Next(5); j++)
            //    {
            //        lst.Add(new() { DateVisiting = DateStart.AddDays(i), Group = new() { Name = "Группа " + i, Color = System.String.Format("#{0:X6}", new Random().Next(0x1000000)) } });
            //    }
            //}
            var DateStart = DateTime.Parse($"{CurrentYear}.{CurrentMounth}.01");
            var DateEnd = DateStart.AddMonths(1).AddDays(5);

            var lst = await _aS.GetAttendancesByDateRange(DateStart.AddDays(-5), DateEnd);
            if (lst.Count() > 0)
            {
                lst = lst.DistinctBy(x => new { x.Group, x.DateVisiting}).ToList();
            }

            var serialize = JsonConvert.SerializeObject(lst, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return new JsonResult(serialize);
        }

    }
}
