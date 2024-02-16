using Core.Accesses;
using Data.Services;
using Data.WebUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security;

namespace TopSkillsWeb.Controllers.Accesses
{
    public class AccessesController : Controller
    {
        private readonly AccessesService _AccessesService;
        private readonly WebUserService _webUser;

        public AccessesController(AccessesService _AccessesService, WebUserService _webUser)
        {
            this._AccessesService = _AccessesService;
            this._webUser = _webUser;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Create = await _webUser.HasAccess(User.Identity.Name, "create", "Accesses");
            return View();
        }

        public async Task<IActionResult> GetRolePermissionDetailPartial(int PermissionId)
        {
            try
            {
                UserPermissions permission = await _AccessesService.GetPermission(PermissionId);
                ViewBag.PermissionName = permission.Description;
                ViewBag.PermissionId = permission.PermissionId;
                var model = await _AccessesService.GetRoles(PermissionId);
                ViewBag.Edit = await _webUser.HasAccess(User.Identity.Name, "edit", "Accesses");
                ViewBag.Delete = await _webUser.HasAccess(User.Identity.Name, "delete", "Accesses");
                bool ExtraAccess = await _webUser.HasExtraAccess(User.Identity.Name);
                if (!ExtraAccess)
                {
                    model.Remove(model.Where(x => x.Name == "OwnerApp").FirstOrDefault());
                }
                return PartialView("RolesListPartial", model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<IActionResult> ShowModalGlobalRolesList()
        {
            bool ExtraAccess = await _webUser.HasExtraAccess(User.Identity.Name);
            var model = await _AccessesService.GetGlobalRolesList(ExtraAccess);
            return PartialView("ModalGlobalRolesList", model);
        }
        public async Task<IActionResult> ShowAddPermissionModal()
        {
            return PartialView("ModalAddPermission");
        }
        public async Task<IActionResult> RenderPermissionList()
        {
            try
            {
                List<UserPermissions> model = await _AccessesService.GetPermissions();
                bool ExtraAccess = await _webUser.HasExtraAccess(User.Identity.Name);
                if (!ExtraAccess)
                {
                    model.Remove(model.Where(x => x.Name == "Accesses").FirstOrDefault());
                }
                return PartialView("PermissionsListPartial", model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IActionResult> CreatePermission(string PermissionCode, string PermissionName)
        {
            try
            {
                UserPermissions model = new();
                model.Name = PermissionCode;
                model.Description = PermissionName;
                bool isExist = await _AccessesService.IsPermissionExists(model);
                if (isExist)
                {
                    return Content("Exist");
                }
                await _AccessesService.CreatePermission(model);
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task TogglePermissionAccessType(int PermissionID, int RoleID, int AccessTypeID)
        {
            try
            {
                await _AccessesService.TogglePermissionAccessType(PermissionID, RoleID, AccessTypeID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }



    }
}
