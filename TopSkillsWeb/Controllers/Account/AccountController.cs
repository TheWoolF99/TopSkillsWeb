﻿using Core.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TopSkillsWeb.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
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
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        
        public async Task<IActionResult> AccountSettings()
        {

            User Person = new()
            {
                UserName = "2423",
                Email = "mail"
            };
            //var avatar = context.UserAvatars.Where(x => x.UserId.Equals(user.Id)).FirstOrDefault();
            //if (avatar != null)
            //{
            //    ViewBag.Avatar = avatar.Avatar;
            //}
            //else
            //{
            //    ViewBag.Avatar = null;
            //}
            //var secretKey = context.UserSecretPhrases.Where(x=>x.UserId.Equals(user.Id)).FirstOrDefault();
            //if(secretKey != null)
            //{
            //    ViewBag.SecretKey = secretKey.SecretPhrase;
            //}
            //else
            //{
            //    ViewBag.SecretKey = null;
            //}

            return View("AccountSettings", Person);
        }


        public async Task<IActionResult> GetTabsSettings(int Tabs)
        {
            switch (Tabs)
            {
                case 0:
                    return PartialView("Tabs/MyAccountTabs", new User() { UserName = "TEs", Email = "TESTSTST@awd.eu" });
                case 3:
                    return PartialView("Tabs/FeedBackTabs", new FeedBackModel());
                default: return PartialView();
            }
        }


        [Authorize]
        public async Task<IActionResult> SaveAccountChanges([Bind] Person profile)
        {
            return new EmptyResult();
            //try
            //{
            //    var user = await _userManager.GetUserAsync(User);                
            //    user.UserName = profile.UserName;
            //    user.Email = profile.Email;
            //    user.Location = profile.Location;
            //    await _userManager.UpdateAsync(user);



            //    return new EmptyResult();
            //}
            //catch (Exception ex)
            //{
            //    return Content($"{ex.Message}");
            //}
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