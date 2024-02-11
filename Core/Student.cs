using Core.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbonementModel = Core.Abonement.Abonement;

namespace Core
{
    public class Student : Person
    {
        /// <summary>
        /// Id студента
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentId { get; set; }
        /// <summary>
        /// Дата создания студента
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreate { get; set; }
        /// <summary>
        /// Ссылка на WebUser на будущее
        /// </summary>
        public User? WebUser { get; set; }
        /// <summary>
        /// Ссылка на группу
        /// </summary>
        public List<Group>? Groups { get; set; }
        /// <summary>
        /// Ссылка на посещения
        /// </summary>
        public IEnumerable<Attendance>? Attendances { get; set; }
        /// <summary>
        /// ФИО родителя
        /// </summary>
        public string? ParentFIO { get; set; }
        /// <summary>
        /// Телефон родителя
        /// </summary>
        public string? ParentPhoneNumber { get; set; }
        /// <summary>
        /// Ссылка на абонемент
        /// </summary>
        public AbonementModel? Abonement { get; set; }
        
        public Student()
        {
            Groups = new List<Group>();
        }
    }
}
