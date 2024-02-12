using Microsoft.AspNetCore.Mvc;
using AttendanceModel = Core.Attendance;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Data.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopSkillsWeb.Resources;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Data.WebUser;

namespace TopSkillsWeb.Controllers.Attendance
{
    [Authorize]
    public class AttendanceController : Controller
    {
        /// <summary>
        /// Сервис для работы с группами
        /// </summary>
        private readonly GroupService _gS;
        private readonly AttendanceService _aS;
        private readonly AbonementService _abonement;
        private readonly WebUserService _webUser;

        public AttendanceController(GroupService groupService, AttendanceService attendanceService, AbonementService abonement, WebUserService webUserService )
        {
            this._gS = groupService;
            this._aS = attendanceService;
            this._abonement = abonement;
            _webUser = webUserService;

        }

        public IActionResult Index()
        {
            return View();
        }



        public async Task<IActionResult> GetModalAddAttendance(string date)
        {
            
            ViewBag.GroupList = new SelectList(await _gS.GetAllGroupsAsync(DateTime.Parse(date)), "GroupId", "Name");
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
            
            var DateStart = DateTime.Parse($"{CurrentYear}.{CurrentMounth}.01");
            var DateEnd = DateStart.AddMonths(1).AddDays(5);

            var lst = await _aS.GetAttendancesByDateRange(DateStart.AddDays(-5), DateEnd);
            if (lst.Count() > 0)
            {
                lst = lst.DistinctBy(x => new { x.Group.GroupId, x.DateVisiting}).ToList();
            }

            var serialize = JsonConvert.SerializeObject(lst, new JsonSerializerSettings() { MaxDepth= Int32.MaxValue, ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            return new JsonResult(serialize);
        }


        public async Task<IActionResult> ShowContextMenu(int id)
        {

            return PartialView("ContextMenu", id);
            return new EmptyResult();
        }

        public async Task<IActionResult> GetListAttendance(DateTime? date)
        {
            date ??= DateTime.Today;
            var lst = await _aS.GetAttendancesByDateRange((DateTime)date);
            if (lst.Count() > 0)
                lst = lst?.DistinctBy(x => new { x.Group.GroupId, x.DateVisiting }).ToList();
            return PartialView("AttendanceTable", lst);
        }

        public async Task<IActionResult> GetStartAttendance(int GroupId, string date)
        {
            
            var lst = await _aS.GetAttendanceByGroupIdAndDate(GroupId, DateTime.Parse(date));
            ViewBag.Title = Resource.AttendanceStart + " - " + lst?.ElementAt(1).Group.Name;
            return PartialView("ModalStartAttendance", lst);
        }

        public async Task<IActionResult> OnStartAttendance(List<AttendanceModel> attendances)
        {
            try
            {
                await _aS.OnStartAttendance(attendances);
                return RedirectToAction("ShowModalSuccess", "Home", new { message = Resource.Saved });
            }
            catch (Exception ex)
            {
                return RedirectToAction("ShowModalError", "Home", new { message = ex.Message });
            }
        }

        public async Task<IActionResult> GetListExpiredStudent()
        {
            ViewBag.UpdateAbonementAccess = await _webUser.HasAccess(User.Identity.Name,"edit","RefreshAbonement");
            return PartialView("ListExpiredAbonement", (await _abonement.GetAllAbonements())?.Where(x => x.RemainingVisits <= 0).ToList());
        }


        public async Task<IActionResult> GetListExpiredGroupStudent(int groupId)
        {
            var StudentsAbonementExpired = (await _abonement.GetAbonementGroupStudents(groupId))?.Where(x => x.RemainingVisits <= 0).ToList();
            
            if(StudentsAbonementExpired.Count>0) 
                return PartialView("ListExpiredAbonement", StudentsAbonementExpired); 
            else
                return new EmptyResult(); 

        }


    }
}
