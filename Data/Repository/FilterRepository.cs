using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class FilterRepository : IFilters
    {
        private DbContextFactory _context;

        public FilterRepository(DbContextFactory context)
        {
            this._context = context;
        }



    }
}
