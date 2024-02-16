using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class ModelMapper
    {
        static string[] IgnoreField = { "datecreate" };


        /// <summary>
        /// Мапер полей
        /// </summary>
        /// <param name="source">Откуда взять значения полей</param>
        /// <param name="target">Куда присвоить значения полей</param>
        public static void UpdateFieldTo(this Student source, Student target)
        {
            target.StudentId = source.StudentId;
            target.WebUser = source.WebUser;
            target.Groups = source.Groups;
            target.Attendances = source.Attendances;
            target.ParentFIO = source.ParentFIO;
            target.ParentPhoneNumber = source.ParentPhoneNumber;
            target.Abonement = source.Abonement;
            target.FirstName = source.FirstName;
            target.MiddleName = source.MiddleName;
            target.LastName = source.LastName;
            target.Age = source.Age;
        }

        /// <summary>
        /// Мапер полей
        /// </summary>
        /// <param name="source">Откуда взять значения полей</param>
        /// <param name="target">Куда присвоить значения полей</param>
        public static void UpdateFieldTo(this Course source, Course target)
        {
            target.CourseId = source.CourseId;
            target.Name = source.Name;
            target.Groups = source.Groups;
        }


        /// <summary>
        /// Мапер полей
        /// </summary>
        /// <param name="source">Откуда взять значения полей</param>
        /// <param name="target">Куда присвоить значения полей</param>
        public static void UpdateFieldTo(this Group source, Group target)
        {
            target.GroupId = source.GroupId;
            target.Name = source.Name;
            target.Cource = source.Cource;
            target.Teacher = source.Teacher;
            target.Color = source.Color;
            target.Students = source.Students;
            target.Attendances = source.Attendances;
        }

        /// <summary>
        /// Мапер полей
        /// </summary>
        /// <param name="source">Откуда взять значения полей</param>
        /// <param name="target">Куда присвоить значения полей</param>
        public static void UpdateFieldTo(this Teacher source, Teacher target)
        {
            target.TeacherId = source.TeacherId;
            target.Courses = source.Courses;
            target.FirstName = source.FirstName;
            target.MiddleName = source.MiddleName;
            target.LastName = source.LastName;
            target.Age = source.Age;
        }



        //public static void Map<TSource, TDestination>(TSource source, TDestination destination)
        //{
        //    var sourceProperties = typeof(TSource).GetProperties();
        //    var destinationProperties = typeof(TDestination).GetProperties();

        //    foreach (var sourceProp in sourceProperties)
        //    {
        //        var destProp = destinationProperties.FirstOrDefault(p => p.Name == sourceProp.Name);
        //        if(destProp != null)
        //        {
        //            if (!IgnoreField.Contains(destProp.Name.ToLower()))
        //            {
        //                if (destProp.PropertyType == sourceProp.PropertyType)
        //                {
        //                    destProp.SetValue(destination, sourceProp.GetValue(source));
        //                }
        //            }
        //        }

        //    }
        //}

    }
}
