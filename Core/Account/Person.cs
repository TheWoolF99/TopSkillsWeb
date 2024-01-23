using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Account
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string? MidleName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public string FullName
        {
            get
            {
                return $"{this.FirstName} {this.MidleName} {this.LastName}";
            }
        }

    }
}
