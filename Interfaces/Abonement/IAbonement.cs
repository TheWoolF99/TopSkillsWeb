using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbonementModel = Core.Abonement.Abonement;

namespace Interfaces.Abonement
{
    public interface IAbonement
    {
        Task<IEnumerable<AbonementModel>> GetAllAbonements();
        Task<AbonementModel?> GetAbonementStudent(int StudentId);
        Task AddNewAbonement(AbonementModel abonement);
        Task UpdateAbonement(AbonementModel abonement);
    }
}
