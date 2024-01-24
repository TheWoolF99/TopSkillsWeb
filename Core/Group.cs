using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupId { get; set; } 
        [NotMapped]
        public string GroupIdStr { get
            {
                return this.GroupId.ToString();
            }
        }
        public string Name { get; set; }
        public Course? Cource { get;set; }
        public Teacher? Teacher { get; set; }
        public IEnumerable<Student>? Students { get; set; }
    }
}
