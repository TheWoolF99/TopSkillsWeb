﻿using Core.Account;
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
using Microsoft.AspNetCore.Authorization;
using TopSkillsWeb.Attributes;
using Data.WebUser;

namespace TopSkillsWeb.Controllers.Group
{
    [Authorize]
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
        private readonly WebUserService _webUser;

        public GroupController(GroupService groupService, CourseService courseService, TeacherService teacherService, StudentService sS, WebUserService webUser)
        {
            this._gS = groupService;
            this._cS = courseService;
            this._tS = teacherService;
            this._sS = sS;
            _webUser = webUser;

        }

        [HasAccess("Group", "read")]
        public async Task<IActionResult> Index()
        {
            string name = User.Identity.Name;
            ViewBag.Edit = await _webUser.HasAccess(name, "edit", "Group");
            ViewBag.Delete = await _webUser.HasAccess(name, "delete", "Course");

            return View(await _gS.GetAllGroupsAsync());
        }

        [HasAccess("Group", "create")]
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


        [HasAccess("Group", "create")]
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

        [HasAccess("Group", "read")]
        public async Task<IActionResult> OnUpdateTableRows()
        {
            string name = User.Identity.Name;
            ViewBag.Edit = await _webUser.HasAccess(name, "edit", "Group");
            ViewBag.Delete = await _webUser.HasAccess(name, "delete", "Group");

            var Group = await _gS.GetAllGroupsAsync();
            return PartialView("RowsPart", Group);
        }



        [HasAccess("Group", "delete")]
        public async Task<IActionResult> ConfirmDeleteGroup(int GroupId)
        {
            return PartialView("ConfirmDelete", GroupId);
        }

        [HasAccess("Group", "delete")]
        public async Task<IActionResult> OnDeleteGroup(int GroupId)
        {
            await _gS.DeleteAsync(GroupId);
            return new EmptyResult();
        }
    }
}
