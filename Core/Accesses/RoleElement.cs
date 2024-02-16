using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Accesses
{
    public class RoleElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }


        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Read { get; set; }
        public bool Delete { get; set; }
        public bool AllAccess { get; set; }
    }
}
