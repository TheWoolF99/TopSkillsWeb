using Core;
using Core.Account;
using Core.Logger;
using Core.Mailer;
using Data.Repository;
using Data.Services;
using Data.WebUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TopSkillsWeb.Attributes;
using UAParser;
//using UAParser;

namespace TopSkillsWeb.Controllers.Account
{

    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly PhotoService _photo;
        private readonly LoggerService _log;
        private readonly GlobalOptionsService _options;
        private readonly WebUserService _webUser;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, PhotoService _photo, LoggerService log, GlobalOptionsService opts, WebUserService webUser)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._photo = _photo;
            this._log = log;
            this._options = opts;
            this._webUser = webUser;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Username };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    //Записываем в лог
                    await _log.AddLog(GetLogItem("Зарегистрировался"));
                    ////запишем в лог
                    //var UserAgentParse = Parser.GetDefault().Parse(HttpContext.Request.Headers.UserAgent).UA;

                    //_log.AddLog(new() { });

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                await _log.AddLog(GetLogItem("Вход"));
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    await _log.AddLog(GetLogItem("Вход"));
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Wrong data");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            //Сначала выходим
            await _signInManager.SignOutAsync();
            //Записываем в лог
            await _log.AddLog(GetLogItem("Выход из системы"));
            ModelState.Clear();
            return RedirectToAction("Login", "Account");
        }

        public LoggerLoginItem GetLogItem(string OperName)
        {
            return new()
            {
                UserId = _userManager.GetUserAsync(User).Result.Id,
                UserName = User.Identity.Name,
                Browser = Parser.GetDefault().Parse(HttpContext.Request.Headers.UserAgent).UA.Family,
                BrowserVer = Parser.GetDefault().Parse(HttpContext.Request.Headers.UserAgent).UA.Major,
                OperationName = OperName
            };
        }

        [Authorize]
        [HasAccess("Logins", "read")]
        public async Task<IActionResult> Logins()
        {
            var model = await _webUser.GetAllUsers();
            return View("Logins/Index", model);
        }

        [Authorize]
        [HasAccess("Logins", "read")]
        public async Task<IActionResult> OnUpdateTableRows()
        {
            var model = await _webUser.GetAllUsers();
            return PartialView("Logins/RowsPart", model);
        }


        [HttpGet]
        [Authorize]
        [HasAccess("Logins", "create")]
        public async Task<IActionResult> GetModalAddLogin()
        {
            return PartialView("Logins/ModalAddLogin");
        }

        [HttpPost]
        [Authorize]
        [HasAccess("Logins", "create")]
        public async Task<IActionResult> CreateNewLogin(string Email, string UserName, string Password)
        {
            if(UserName.Length < 5)
            {
                return RedirectToAction("ShowModalError", "Home", new { message = Resources.Resource.UserNameLengthError });
            }
            if(Password.Length < 5)
            {
                return RedirectToAction("ShowModalError", "Home", new { message = Resources.Resource.PasswordLengthError });
            }

            User user = new User { Email = Email, UserName = UserName };
            // добавляем пользователя
            var result = await _userManager.CreateAsync(user, Password);
            if (result.Succeeded)
            {
                //Записываем в лог
                await _log.AddLog(GetLogItem($"Зарегистрировал пользователя - {user.UserName}"));
                return new EmptyResult();
            }
            return RedirectToAction("ShowModalError", "Home",  new { message = Resources.Resource.AnErrorHasOccurred});
        }

        [Authorize]
        [HasAccess("Accesses", "edit")]
        public async Task<IActionResult> GetRoleListForUser(string UserName) 
        {
            var model = await _webUser.GetAllRoles();
            if(User.Identity.Name != "OwnerApp")
            {
                model = model.Where(x => x.NormalizedName != "OwnerApp".ToUpper()).ToList();
            }
            ViewBag.UserRoles = await _userManager.GetRolesAsync(await _userManager.FindByNameAsync(UserName));
            ViewBag.UserName = UserName;
            return PartialView("Logins/ModalAddRoles", model);
        }

        [Authorize]
        [HasAccess("Accesses", "edit")]
        public async Task<IActionResult> SetRoleUser(string RoleName, string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);

            if ((await _userManager.GetRolesAsync(user)).Contains(RoleName))
            {
                var res = await _userManager.RemoveFromRoleAsync(user, RoleName);
                return RedirectToAction("ShowModalSuccess", "Home", new { message = Resources.Resource.RoleDeleted });
            }
            else
            {
                var res = await _userManager.AddToRoleAsync(user, RoleName);
                return RedirectToAction("ShowModalSuccess", "Home", new { message = Resources.Resource.RoleAdded });
            }
        }
        



        [Authorize]
        public async Task<IActionResult> AccountSettings()
        {
            User? currentUser = await _userManager.GetUserAsync(User);
            string? userId = currentUser?.Id;
            ViewBag.Avatar = await _photo.GetAvatarUser(userId ?? "");
            return View("AccountSettings", currentUser);
        }


        public async Task<IActionResult> GetTabsSettings(int Tabs)
        {
            switch (Tabs)
            {
                case 0:
                    ViewBag.Avatar = await _photo.GetAvatarUser((await _userManager.GetUserAsync(User)).Id ?? "");
                    return PartialView("Tabs/MyAccountTabs", await _userManager.GetUserAsync(User));
                case 1:
                    var RolesPermissions = await _webUser.GetUserRolesPermissions(User.Identity.Name);
                    return PartialView("Tabs/MyRolesAndPermissions", RolesPermissions);
                case 2:
                    if (User.IsInRole("OwnerApp")) 
                    {
                        var opt = await _options.GetMailOptionAsync();
                        return PartialView("Tabs/MailOptions", opt);
                    }
                    else { return RedirectToAction("AccessDenied", "Home");}
                case 3:
                    return PartialView("Tabs/FeedBackTabs", new FeedBackModel());
                default: return PartialView();
            }
        }

        [Authorize(Roles = "OwnerApp")]
        public async Task<IActionResult> OnChangeMailerSetting(MailOption model)
        {

            foreach (var item in model.GetType().GetProperties())
            {
                if(item.Name == "SMTPLogin" | item.Name == "SMTPPassword")
                {
                    await _options.UpdateOptions(item.Name, AES_128_ECB.Encrypt_AES_128_ECB(item.GetValue(model).ToString()));
                }
                else
                {
                    await _options.UpdateOptions(item.Name, item.GetValue(model).ToString());
                }
            }
            return RedirectToAction("ShowModalSuccess", "Home", new { message = Resources.Resource.Saved });
        }



        [Authorize]
        public async Task<IActionResult> SaveAccountChanges()
        {
            var FilesList = Request.Form.Files;
            User profile = JsonConvert.DeserializeObject<User>(Request.Form["Model"]);

            try
            {

                var user = await _userManager.GetUserAsync(User);
                user.UserName = profile.UserName;
                user.Email = profile.Email;
                await _userManager.UpdateAsync(user);

                if (FilesList.Any())
                {
                    var Avatar = FilesList[0];
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(Avatar.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)Avatar.Length);
                    }
                    if (imageData != null)
                        await _photo.OnAddUpdateAvatarUser(user.Id, imageData);
                }



                return new EmptyResult();
            }
            catch (Exception ex)
            {
                return Content($"{ex.Message}");
            }
        }


        [Authorize]
        public async Task<IActionResult> UploadAvatar()
        {
            return new EmptyResult();
        }

        [Authorize]
        public async Task<IActionResult> IsEmailFree(string email)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser.Email.Equals(email))
            {
                return new EmptyResult();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return Content("Email занят");
            }

            return new EmptyResult();
        }
        [Authorize]
        public async Task<IActionResult> ShowModalResetAvatarConfirmation()
        {
            return PartialView("ModalResetAvatarConfirmation");
        }
        [Authorize]
        public async Task<IActionResult> ResetAvatar()
        {
            return new EmptyResult();
            //try
            //{
            //    var currentUser = await _userManager.GetUserAsync(User);
            //    var avatar = context.UserAvatars.Where(x=>x.UserId.Equals(currentUser.Id)).FirstOrDefault();
            //    if (avatar != null)
            //    {
            //        context.UserAvatars.Remove(avatar);
            //        await context.SaveChangesAsync();
            //    }
            //    return new EmptyResult();
            //}
            //catch(Exception ex)
            //{
            //    return Content(ex.Message);
            //}
        }

        [Authorize]
        public async Task<IActionResult> ShowModalResetPassword()
        {
            return PartialView("ModalResetPassword");
        }

        [Authorize]
        public async Task<IActionResult> ChangeUserPassword(string Password)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, code, Password);

                return new EmptyResult();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [Authorize]
        public async Task<IActionResult> ShowModalSecretKey()
        {
            return PartialView("ModalSecretKey");
        }

        [Authorize]
        public async Task<IActionResult> AddUpdateSecretPhrase(string SecretPhrase)
        {
            return new EmptyResult();
            //try
            //{
            //    var user = await _userManager.FindByNameAsync(User.Identity.Name);
            //    var secretKey = context.UserSecretPhrases.Where(x => x.UserId.Equals(user.Id)).FirstOrDefault();
            //    if(secretKey != null)
            //    {
            //        secretKey.SecretPhrase = SecretPhrase;
            //    }
            //    else
            //    {
            //        UserSecretPhrase userSecretPhrase = new UserSecretPhrase();
            //        userSecretPhrase.UserId = user.Id;
            //        userSecretPhrase.SecretPhrase = SecretPhrase;
            //        await context.UserSecretPhrases.AddAsync(userSecretPhrase);
            //    }
            //    await context.SaveChangesAsync();
            //    return new EmptyResult();
            //}
            //catch (Exception ex)
            //{
            //    return Content(ex.Message);
            //}
        }
    }
}
