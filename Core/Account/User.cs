using Microsoft.AspNetCore.Identity;

namespace Core.Account
{
    public class User: IdentityUser
    {
        public string RoleName { get; set; } = "";
    }
}
