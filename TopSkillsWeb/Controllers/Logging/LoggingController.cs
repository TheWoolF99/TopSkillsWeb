using Core.Logger;
using Data.Services;
using Data.WebUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopSkillsWeb.Attributes;

namespace TopSkillsWeb.Controllers.Logging
{
    [Authorize]
    public class LoggingController : Controller
    {
        private readonly WebUserService _webUser;
        private readonly LoggerService _log;

        public LoggingController(WebUserService webUser, LoggerService log)
        {
            _webUser = webUser;
            _log = log;
        }

        [HasAccess("Logging", "read")]
        public IActionResult Index()
        {
            return View();
        }

        [HasAccess("Logging", "read")]
        public async Task<IActionResult> GetLogAuth(DateTime? DateStart, DateTime? DateEnd, string? UserName)
        {
            LoggerFilter filter = new();
            if (DateStart!=null & DateEnd != null)
            {
                filter.DateStart = DateStart.Value;
                filter.DateEnd = DateEnd.Value;
            }
            filter.UserName = UserName;

            var lst = await _log.GetLogsAuthWithFilter(filter);
            return PartialView("LogAuth/TableAuth", lst);
        }

    }
}
