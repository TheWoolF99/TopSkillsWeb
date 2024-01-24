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
        public string? MidleName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{this.FirstName} {this.MidleName} {this.LastName}";
            }
        }

    }
}
