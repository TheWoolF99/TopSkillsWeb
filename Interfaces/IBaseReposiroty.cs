using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IBaseReposiroty<T>
    {
        bool Create(T entity);

        T GetById(int id);

        IQueryable<T> GetAll();

        bool Delete(T entity);
    }
}
