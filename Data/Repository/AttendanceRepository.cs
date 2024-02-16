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
            var Attendance = await db.Attendance.Include(s => s.Student).ThenInclude(a=>a.Abonement).Include(g => g.Group).Where(x=>x.Group.GroupId == GroupId & x.DateVisiting == Date).ToListAsync();
            return Attendance;
        }

        public async Task<bool> OnAddAttendanceByDateAndGroupId(Attendance attendance)
        {
            var db = _context.Create(typeof(AttendanceRepository));
            var Group = db.Groups.Include(s=>s.Students).ThenInclude(a=>a.Abonement).Where(x => x.GroupId == attendance.Group.GroupId).FirstOrDefault();
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

            var Atts = await db.Attendance
                .Include(g => g.Group).Where(x => x.DateVisiting >= start & x.DateVisiting <= end)
                .Select(x => new Attendance{
                    AttendanceId = x.AttendanceId,
                    DateClose = x.DateClose,
                    DateVisiting = x.DateVisiting,
                    Group = new Group() { GroupId = x.Group.GroupId, Color = x.Group.Color, Name = x.Group.Name }
                })
                .OrderBy(x => x.DateClose)
                .ToListAsync();
            return Atts;
        }

        public async Task<IEnumerable<Attendance>> GetAttendancesByDateRange(DateTime date)
        {
            var db = _context.Create(typeof(AttendanceRepository));
            return await db.Attendance
                .Include(g => g.Group).Include(s => s.Student).Where(x => x.DateVisiting.Date == date.Date).ToListAsync();
        }

        public async Task OnStartAttendance(List<Attendance> attendances)
        {
            var db = _context.Create(typeof(AttendanceRepository));
            var attendanceIds = attendances.Select(x => x.AttendanceId);
            var attendancesDB = db.Attendance.Include(s => s.Student).Include(g => g.Group).Where(x => attendanceIds.Contains(x.AttendanceId)).ToList();
            var abonements = db.Abonements.ToList();
            attendancesDB.ForEach(x =>
            {
                x.DateClose = DateTime.Now;
                var attendanceNew = attendances.Where(a => a.AttendanceId == x.AttendanceId).FirstOrDefault();
                ///вычтем занятие из абонемента если IsPresent не 2(по уважительной причине)
                if (attendanceNew!=null && attendanceNew.IsPresent != 2)
                {
                    var abonement = abonements.Where(ab => ab.StudentId == x.Student.StudentId).FirstOrDefault();
                    if(abonement!=null && !(abonement.RemainingVisits <= 0))
                        abonement.RemainingVisits = --abonement.RemainingVisits;
                }
                x.IsPresent = attendanceNew != null ? attendanceNew.IsPresent : 0;
            });
            await db.SaveChangesAsync();
        }





    }
}
