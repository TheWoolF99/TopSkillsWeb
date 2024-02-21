using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security;
using Data.WebUser;
using Core.Accesses;
using Microsoft.Extensions.DependencyInjection;

namespace TopSkillsWeb.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class HasAccessAttribute : ActionFilterAttribute
    {
        private readonly string _AccessName;
        private readonly string _AccessType;

        public HasAccessAttribute(string AccessName, string AccessType)
        {
            _AccessName = AccessName;
            _AccessType = AccessType;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Получение userId из контекста пользователя Identity
            //string userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var User = context.HttpContext.User.Identity.Name;
            var serviceProvider = context.HttpContext.RequestServices;
            var _webUser = serviceProvider.GetRequiredService<WebUserService>();

            // Проверка доступа пользователя к функциональности по разрешению
            if (!_webUser.HasAccess(User, _AccessType, _AccessName).Result)
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
