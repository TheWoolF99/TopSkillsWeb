using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class AccessesRepository : IAccessesRepository
    {
        private readonly DbContextFactory dbContextFactory;

        public AccessesRepository(DbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }
    }
}
