using Core.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace TopSkillsWeb.ViewComponents
{
    public class UserComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;

        public UserComponent(UserManager<User> _userManager)
        {
            this._userManager = _userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string NameUser = HttpContext.User.Identity.Name;
            User? appUser = new();
            IList<string> roles = new List<string>();

            if (!string.IsNullOrEmpty(NameUser))
            {
                appUser = await _userManager.FindByNameAsync(NameUser);
                roles = await _userManager.GetRolesAsync(appUser);
            }

            User user1 = new()
            {
                UserName = appUser.UserName ??= "EmptyName",
                RoleName = roles.Count()==0? "EmptyRole" :string.Join(" , ", roles.ToArray())
            };

            return View(user1);
        }
        
    }
}
