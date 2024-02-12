using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Accesses
{
    public class AccessTypes
    {
        [Key]
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public AccessTypes(int typeId, string name, string code)
        {
            TypeId = typeId;
            Name = name;
            Code = code;
        }
    }
}
