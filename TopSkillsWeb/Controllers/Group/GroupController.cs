using Core.Account;
using Data.Repository;
using Data.Services;
using GroupModel = Core.Group;
using StudentModel = Core.Student;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopSkillsWeb.Resources;
using Core;
using Newtonsoft.Json;
using Interfaces.Course;

namespace TopSkillsWeb.Controllers.Group
{
    public class GroupController : Controller
    {
        /// <summary>
        /// Сервис для работы с группами
        /// </summary>
        private readonly GroupService _gS;
        /// <summary>
        /// Сервис для работы с курсами
        /// </summary>
        private readonly CourseService _cS;
        /// <summary>
        /// Сервис для работы с преподавателями
        /// </summary>
        private readonly TeacherService _tS;
        /// <summary>
        /// Сервис для работы с студентами
        /// </summary>
        private readonly StudentService _sS;

        public GroupController(GroupService groupService, CourseService courseService, TeacherService teacherService, StudentService sS)
        {
            this._gS = groupService;
            this._cS = courseService;
            this._tS = teacherService;
            this._sS = sS;
        }


        public IActionResult Index()
        {
            return View(_gS.GetAllGroupsAsync().Result);
        }


        public async Task<IActionResult> GetModalAddEditGroup(int? GroupId = null)
        {
            ViewBag.CourseList = new SelectList(await _cS.GetAllCoursesAsync(), "CourseId", "Name");
            ViewBag.TeacherList = new SelectList(await _tS.GetAllTeachersAsync(), "TeacherId", "FullName");
            ViewBag.StudentsList = await _sS.GetAllStudentsAsync();

            GroupModel group = new();
            ViewBag.Title = Resource.CreateGroup;
            if (GroupId != null)
            {
                ViewBag.Title = Resource.EditGroup;
                group = await _gS.GetGroupAsync((int)GroupId);
            }

            //ViewBag.Serialize =  JsonConvert.SerializeObject(group);


            return PartialView("ModalNewGroup", group);
        }


        public async Task<IActionResult> OnAddGroupStudents(GroupModel Group, List<int> StudentsIds)
        {
            bool add = Group.GroupId == 0;
            if (add)
            {
                int NewId = _gS.AddGroupAsync(Group).Result;
                await _gS.UpdateGroupWithStudents(NewId, StudentsIds);
            }
            else
            {
                await _gS.Update(Group);
                await _gS.UpdateGroupWithStudents(Group.GroupId, StudentsIds);
            }
            return RedirectToAction("ShowModalSuccess", "Home", new { message = add ? Resource.GroupAddDone : Resource.GroupEditDone });
        }

        public async Task<IActionResult> OnUpdateTableRows()
        {
            var Group = await _gS.GetAllGroupsAsync();
            return PartialView("RowsPart", Group);
        }

    }
}
