using Core.Account;
using Data.Repository;
using Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace TopSkillsWeb.Controllers.Group
{
    public class GroupController : Controller
    {
        private readonly GroupService _gS;

        public GroupController(GroupService groupService)
        {
           this._gS = groupService;
        }


        public IActionResult Index()
        {

            return View(_gS.GetAllGroupsAsync().Result);
        }
    }
}
