using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Attendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttendanceId { get; set; } 
        public DateTime DateVisiting {  get; set; }
        public Student Student { get; set; }
        public Group Group { get; set; }
        public int IsPresent { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreate { get; set; }
        public DateTime? DateClose { get; set; }
    }
}
