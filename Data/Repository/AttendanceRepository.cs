using Core;
using Interfaces.Attendance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class AttendanceRepository : IAttendance
    {
        private DbContextFactory _context;

        public AttendanceRepository(DbContextFactory context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Attendance>> GetAttendanceByGroupIdAndDate(int GroupId, DateTime Date)
        {
            var db = _context.Create(typeof(AttendanceRepository));
            var Attendance = await db.Attendance.Include(s => s.Student).Include(g => g.Group).Where(x=>x.Group.GroupId == GroupId & x.DateVisiting == Date).ToListAsync();
            return Attendance;
        }

        public async Task<bool> OnAddAttendanceByDateAndGroupId(Attendance attendance)
        {
            var db = _context.Create(typeof(AttendanceRepository));
            var Group = db.Groups.Include(s=>s.Students).Where(x => x.GroupId == attendance.Group.GroupId).FirstOrDefault();
            List<Attendance> attendances = new();
            foreach (var item in Group.Students)
            {
                attendances.Add(new()
                {
                    DateVisiting = attendance.DateVisiting,
                    Group = Group,
                    Student = item
                });
            }
            try
            {
                if(db.Attendance.Where(x=>x.Group == Group && x.DateVisiting == attendance.DateVisiting).Count() == 0)
                {
                    await db.Attendance.AddRangeAsync(attendances);
                    await db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    throw new Exception("На данную дату уже есть посещение этой группы");
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }



        public async Task<IEnumerable<Attendance>> GetAttendancesByDateRange(DateTime start, DateTime end)
        {
            var db = _context.Create(typeof(AttendanceRepository));
            return await db.Attendance
                .Include(g => g.Group).Include(s => s.Student).Where(x => x.DateVisiting >= start & x.DateVisiting <= end).ToListAsync();
                //.DistinctBy(x => x.Group)
                //.AsEnumerable();
        }

        public async Task<IEnumerable<Attendance>> GetAttendancesByDateRange(DateTime date)
        {
            var db = _context.Create(typeof(AttendanceRepository));
            return await db.Attendance
                .Include(g => g.Group).Include(s => s.Student).Where(x => x.DateVisiting.Date == date.Date).ToListAsync();
            //.DistinctBy(x => x.Group)
            //.AsEnumerable();
        }

        public async Task OnStartAttendance(List<Attendance> attendances)
        {
            var db = _context.Create(typeof(AttendanceRepository));
            var attendanceIds = attendances.Select(x => x.AttendanceId);
            var attendancesDB = db.Attendance.Where(x => attendanceIds.Contains(x.AttendanceId));
            await attendancesDB.ForEachAsync(x =>
            {
                x.DateClose = DateTime.Now;
                var attendanceNew = attendances.Where(a => a.AttendanceId == x.AttendanceId).FirstOrDefault();
                x.IsPresent = attendanceNew != null ? attendanceNew.IsPresent : 0;
            });
            await db.SaveChangesAsync();
        }


    }
}
