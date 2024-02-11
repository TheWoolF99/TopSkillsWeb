using AbonementModel = Core.Abonement.Abonement;
using Interfaces.Abonement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Abonement;
using Microsoft.EntityFrameworkCore;

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

        public async Task AddNewAbonement(AbonementModel abonement)
        {
            var db = _context.Create(typeof(AbonementRepository));
            await db.Abonements.AddAsync(abonement);
            await db.SaveChangesAsync();
        }

        public async Task UpdateAbonement(AbonementModel abonement)
        {
            var db = _context.Create(typeof(AbonementRepository));
            db.Abonements.Update(abonement);
            await db.SaveChangesAsync();
        }


    }
}
