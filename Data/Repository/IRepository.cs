using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public interface IRepository <T> where T : class
    {
        IQueryable<T> GetAll();
        Task<IQueryable<T>> GetAllAsync();
        Task<T> GetAsync(string id);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
    }
}
