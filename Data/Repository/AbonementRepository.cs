using AbonementModel = Core.Abonement.Abonement;
using Interfaces.Abonement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Abonement;
using Microsoft.EntityFrameworkCore;
using Core;

namespace Data.Repository
{
    public class AbonementRepository : IAbonement
    {
        private DbContextFactory _context;

        public AbonementRepository(DbContextFactory context) =>
            this._context = context;


        public async Task<IEnumerable<AbonementModel>> GetAllAbonements()
        {
            var db = _context.Create(typeof(AbonementRepository));
            return await db.Abonements.Include(s=>s.Student).ToListAsync();
        }

        public async Task<AbonementModel?> GetAbonementStudent(int StudentId)
        {
            var db = _context.Create(typeof(AbonementRepository));
            return db.Abonements.Where(x => x.StudentId == StudentId).FirstOrDefault();
        }

        public async Task<IEnumerable<AbonementModel>> GetAbonementGroupStudents(int groupId)
        {
            var db = _context.Create(typeof(AbonementRepository));
            var Group = await db.Groups.Include(s=>s.Students).Where(g=>g.GroupId == groupId).FirstOrDefaultAsync();
            List<AbonementModel> result = new();
            if(Group != null)
            {
                
                result = await db.Abonements.Where(x => Group.Students.Select(x=>x.StudentId).Contains(x.StudentId)).ToListAsync();
            }

            return result;
        }

        public async Task AddNewAbonement(AbonementModel abonement)
        {
            var db = _context.Create(typeof(AbonementRepository));
            var StandartRemainingVisits = db.GlobalOptions.Where(x => x.OptionName.ToLower() == "StandartRemainingVisits".ToLower()).FirstOrDefault()?.OptionValue;
            StandartRemainingVisits ??= "4";
            abonement.RemainingVisits = Convert.ToInt32(StandartRemainingVisits);
            await db.Abonements.AddAsync(abonement);
            await db.SaveChangesAsync();
        }

        public async Task UpdateAbonement(AbonementModel abonement)
        {
            var db = _context.Create(typeof(AbonementRepository));
            db.Abonements.Update(abonement);
            await db.SaveChangesAsync();
        }


        public async Task UpdateCountAbonementByStudentId(int studentId, int countVisits)
        {
            var db = _context.Create(typeof(AbonementRepository));
            var abonement = await db.Abonements.FirstOrDefaultAsync(x=>x.StudentId == studentId);
            if(abonement != null)
                abonement.RemainingVisits = countVisits;

            await db.SaveChangesAsync();
        }

        public async Task RefreshAbonement(int StudentId)
        {
            var db = _context.Create(typeof(AbonementRepository));
            var abonement = db.Abonements.Where(x => x.StudentId == StudentId).FirstOrDefault();
            var StandartRemainingVisits = db.GlobalOptions.Where(x=>x.OptionName.ToLower() == "StandartRemainingVisits".ToLower()).FirstOrDefault()?.OptionValue;
            StandartRemainingVisits ??= "4";
            if (abonement == null)
                throw new Exception("Не найден абонемент студента");
            if (abonement.RemainingVisits != 0)
                throw new Exception("У студента еще не закончился абонемент");
            if (abonement != null)
            {
                abonement.RemainingVisits = Convert.ToInt32(StandartRemainingVisits);
                abonement.StartDate = DateTime.Now;
            }
            await db.SaveChangesAsync();
        }

    }
}
