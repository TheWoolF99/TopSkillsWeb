using Data.Services;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace TopSkillsWeb.Models
{
    public static class UserExtension
    {
        /// <summary>
        /// Всегда возвращает true
        /// </summary>
        /// <param name="User"></param>
        /// <param name="AccessName"></param>
        /// <param name="AccessType"></param>
        /// <returns></returns>
        public static bool HasAccess(this ClaimsPrincipal User, string AccessName, string AccessType)
        {
            return true;
        }

    }
}
