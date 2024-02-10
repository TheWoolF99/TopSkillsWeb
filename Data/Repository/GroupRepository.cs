using Core;
using Interfaces.Group;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

            var ls = db.Groups.Include(s=>s.Students)
                .Include(c=>c.Cource)
                .Include(t=>t.Teacher).ToList();
            //Выбираем со связью
            return ls;
        }

        public async Task<IEnumerable<Group>> GetAllGroupsAsync(DateTime date)
        {
            var db = _context.Create(typeof(GroupRepository));
            var AttendanceToday = db.Attendance.Where(x=>x.DateVisiting == date).Include(x=>x.Group).Select(x=>x.Group.GroupId).ToList()?.Distinct();
            var ls = db.Groups.Include(s => s.Students)
                .Include(c => c.Cource)
                .Include(t => t.Teacher).Where(x => !AttendanceToday.Contains(x.GroupId)).ToList();
            return ls;
        }

        public async Task<Group> GetGroupAsync(int id)
        {
            var db = _context.Create(typeof(GroupRepository));
            var lst = await db.Groups.Include(s => s.Students)
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
            var group = db.Groups.Include(g => g.Students).FirstOrDefault(g => g.GroupId == groupId);
            var Students = db.Students.Where(s => StudentsIds.Contains(s.StudentId)).ToList();

            if (group != null)
            {
                group.Students = Students;
                await db.SaveChangesAsync();
            }
        }


    }




}
