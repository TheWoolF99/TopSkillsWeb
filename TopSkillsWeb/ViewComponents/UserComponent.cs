using Core.Account;
using Data.Repository;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace TopSkillsWeb.ViewComponents
{
    [Authorize]
    public class UserComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;
        private readonly PhotoService _photo;

        public UserComponent(UserManager<User> _userManager, PhotoService _photo)
        {
            this._userManager = _userManager;
            this._photo = _photo;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var contextUser = HttpContext.User;
            User? currentUser = await _userManager.GetUserAsync(contextUser);
            IList<string> currentUserRoles = await _userManager.GetRolesAsync(currentUser);
            currentUser.RoleName = string.Join(", ", currentUserRoles.ToArray());
            currentUser.Avatar = await _photo.GetAvatarUser(currentUser.Id);

            return View(currentUser);
        }
        
    }
}
