using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Account
{
    public class User: IdentityUser
    {
        [NotMapped]
        public string RoleName { get; set; } = "";
        [NotMapped]
        public byte[]? Avatar { get; set; }
    }
}
