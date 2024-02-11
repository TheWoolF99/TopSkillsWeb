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
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; } = 0;

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{this.FirstName} {this.MiddleName} {this.LastName}";
            }
        }
        [NotMapped]
        public string ShortName
        {
            get
            {
                return $"{this.FirstName} {this.LastName}";
            }
        }
        [NotMapped]
        public string FullNameIinitials
        {
            get
            {
                return $"{this?.LastName} {this.MiddleName?[0]} {this.FirstName?[0]} ";
            }
        }

    }
}
