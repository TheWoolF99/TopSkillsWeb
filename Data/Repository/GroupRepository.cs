using Core;
using Interfaces.Attendance;
using Interfaces.Group;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;
using Group = Core.Group;

namespace Data.Repository
{
    public class GroupRepository : IGroup
    {
        private DbContextFactory _context;

        public GroupRepository(DbContextFactory context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Group>> GetAllGroupsAsync()
        {
            var db = _context.Create(typeof(GroupRepository));

            var ls = db.Groups.Include(s=>s.Students).ThenInclude(a=>a.Abonement)
                .Include(c=>c.Cource)
                .Include(t=>t.Teacher).ToList();
            //Выбираем со связью
            return ls;
        }

        public async Task<IEnumerable<Group>> GetAllGroupsAsync(DateTime date)
        {
            var db = _context.Create(typeof(GroupRepository));
            var AttendanceToday = db.Attendance.Where(x=>x.DateVisiting == date).Include(x=>x.Group).Select(x=>x.Group.GroupId).ToList()?.Distinct();
            var ls = db.Groups.Include(s => s.Students).ThenInclude(a => a.Abonement)
                .Include(c => c.Cource)
                .Include(t => t.Teacher).Where(x => !AttendanceToday.Contains(x.GroupId)).ToList();
            return ls;
        }

        public async Task<Group> GetGroupAsync(int id)
        {
            var db = _context.Create(typeof(GroupRepository));
            var lst = await db.Groups.Include(s => s.Students).ThenInclude(a => a.Abonement)
                .Include(c => c.Cource)
                .Include(x=>x.Teacher).SingleAsync(x => x.GroupId == id);
            return lst;
        }


        public async Task<int> AddGroupAsync(Group group)
        {
            var db = _context.Create(typeof(GroupRepository));
            if (group.Cource != null)
            {
                group.Cource = db.Courses.Where(x => x.CourseId == group.Cource.CourseId).FirstOrDefault();
            }
            if (group.Teacher != null)
            {
                group.Teacher = db.Teachers.Where(x => x.TeacherId == group.Teacher.TeacherId).FirstOrDefault();
            }
            
            await db.Groups.AddAsync(group);
            await db.SaveChangesAsync();
            return group.GroupId;
        }

        public async Task Update(Group group)
        {
            var db = _context.Create(typeof(GroupRepository));
            if (group.Cource != null)
            {
                group.Cource = db.Courses.Where(x=>x.CourseId == group.Cource.CourseId).FirstOrDefault();
            }
            if (group.Teacher != null)
            {
                group.Teacher = db.Teachers.Where(x => x.TeacherId == group.Teacher.TeacherId).FirstOrDefault();
            }
            db.Groups.Update(group);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var db = _context.Create(typeof(GroupRepository));
            await db.Groups.Where(x => x.GroupId == id).DeleteAsync();
            await db.SaveChangesAsync();
        }



        public async Task AddGroupStudentsAsync(IEnumerable<GroupStudent> groupStudents)
        {
            var db = _context.Create(typeof(GroupRepository));
            
            await db.SaveChangesAsync();
        }

        public async Task UpdateGroupWithStudents(int groupId, List<int> StudentsIds)
        {
            var db = _context.Create(typeof(GroupRepository));
            //найдем нужную нам группу
            var group = db.Groups.Include(g => g.Students).FirstOrDefault(g => g.GroupId == groupId);
            //получим студентов которых мы добавляем
            var Students = db.Students.Where(s => StudentsIds.Contains(s.StudentId)).ToList();
            //Если удалось найти группу
            if (group != null)
            {
                group.Students = Students;

                //Найдем есть ли будущие посещения у группы
                var Attendance = db.Attendance.Where(x => x.Group.GroupId == group.GroupId & x.DateVisiting.Date >= DateTime.Now.Date & x.DateClose == null).ToList();
                //Если есть, необходимо отредактировать их с учетом студентов.
                if (Attendance.Count > 0)
                {
                    //Получим студентов у которых еще нет посещений. Но их добавили в группу
                    var StudensWithOutAttendance = Students.Except(Attendance.Select(x => x.Student)).ToList();
                    //Подготовим список
                    List<Attendance> attendances = new();
                    //Пройдемся по датам будущих посещений, и создадим запись о новых студентах
                    foreach (var st in Attendance.GroupBy(x => x.DateVisiting))
                    {
                        foreach (var item in StudensWithOutAttendance)
                        {
                            attendances.Add(new()
                            {
                                DateVisiting = st.Key,
                                Group = group,
                                Student = item
                            });
                        }
                    }
                    db.Attendance.AddRange(attendances);
                    //Найдем посещение студента которого удалили из группы
                    //Если удалили студента из группы, надо удалить и его посещения, если они есть
                    var AttendanceWithOutStudens = Attendance.Select(x => x.Student).Except(Students).ToList();
                    //Найдем посещения которые надо удалить
                    var AttendanceIds = Attendance.Where(x => AttendanceWithOutStudens.Contains(x.Student)).ToList();
                    if (AttendanceIds.Count > 0)
                    {
                        db.Attendance.DeleteRangeByKey(AttendanceIds);
                    }
                }
            }
            else
            {
                throw new Exception("Группа не найдена");
            }
            await db.SaveChangesAsync();
        }
    }




}
