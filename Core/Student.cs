using Core.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Student : Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentId { get; set; }
        public DateTime AbonimentStart { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreate { get; set; }
        public int Visits { get; set; } 
        public User? WebUser { get; set; }


        public List<Group>? Groups { get; set; }
        public IEnumerable<Attendance>? Attendances { get; set; }

        public Student()
        {
            Groups = new List<Group>();
        }
    }
}
