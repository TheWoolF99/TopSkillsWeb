using Core;
using Core.Abonement;
using Interfaces.Abonement;
using Interfaces.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class AbonementService
    {
        private readonly IAbonement _abonement;
        public AbonementService(IAbonement abonement) => this._abonement = abonement;

        public async Task<IEnumerable<Abonement>> GetAllAbonements()
        {
            return await _abonement.GetAllAbonements();
        }

        public async Task<Abonement?> GetAbonementStudent(int StudentId)
        {
            return await _abonement.GetAbonementStudent(StudentId);
        }
        public async Task<IEnumerable<Abonement>> GetAbonementGroupStudents(int groupId)
        {
            return await _abonement.GetAbonementGroupStudents(groupId);
        }

        public async Task AddNewAbonement(Abonement abonement)
        {
            await _abonement.AddNewAbonement(abonement);
        }
        public async Task UpdateAbonement(Abonement abonement)
        {
            await _abonement.UpdateAbonement(abonement);
        }

        public async Task RefreshAbonement(int StudentId)
        {
            await _abonement.RefreshAbonement(StudentId);
        }

    }
}
