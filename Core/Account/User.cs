using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Account
{
    public class User: IdentityUser
    {
        public int? StudentId { get; set; }
        public Student? Student { get; set; }
        public int? TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        [NotMapped]
        public string RoleName { get; set; } = "";
        [NotMapped]
        public byte[]? Avatar { get; set; }
        public int Active { get; set; } = 1;
    }
}
