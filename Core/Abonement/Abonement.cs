using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abonement
{
    public class Abonement
    {
        [Key]
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public int RemainingVisits { get; set; }
        public DateTime StartDate { get; set; }
        public Student Student { get; set; }

        public Abonement()
        {
            RemainingVisits = 4;
            StartDate = DateTime.Now;
        }
    }
}
