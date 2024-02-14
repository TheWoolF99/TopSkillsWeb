using Core.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    
    public class AspNetUserRolesTable : IdentityUserRole<string>
    {
        [NotMapped]
        public string UserId { get; set; }
        [NotMapped]
        public string RoleId { get; set; }
    }
}
