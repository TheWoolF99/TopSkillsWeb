using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Accesses
{
    public class RolePermissions
    {
        [Key]
        public int ItemID { get; set; }
        public int RoleId { get; set; }
        public int PermisionId { get; set; }
        public int AccessTypeId { get; set; }
    }
}
