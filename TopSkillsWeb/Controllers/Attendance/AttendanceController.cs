using Microsoft.AspNetCore.Mvc;
using AttendanceModel = Core.Attendance;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TopSkillsWeb.Controllers.Attendance
{
    public class AttendanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public async Task<JsonResult> GetCalendarData(string CurrentYear, string CurrentMounth)
        {
            List<AttendanceModel> lst = new();
            var DateStart = DateTime.Parse("01.01.2024");

            for (int i = 0;i<31; i++)
            {
                lst.Add(new() { DateVisiting = DateStart.AddDays(i), Group = new() { Name="Группа " + i , Color = System.String.Format("#{0:X6}", new Random().Next(0x1000000)) } });
            }
            return new JsonResult(lst);
        }

    }
}
