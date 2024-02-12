using Core;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class GlobalOptionsRepository : IGlobalOptions
    {
        private DbContextFactory _context;

        public GlobalOptionsRepository(DbContextFactory context)
        {
            this._context = context;
        }


        public async Task<IEnumerable<GlobalOptions>> GetAllOptions(string? search)
        {
            var db = _context.Create(typeof(GlobalOptionsRepository));
            var result = new List<GlobalOptions>();

            if(search!=null)
                result = await db.GlobalOptions.Where(x => x.OptionName.Contains(search)).ToListAsync();
            else
                result = await db.GlobalOptions.ToListAsync();
            return result;
        }


        public async Task<GlobalOptions?> GetOptionsByName(string OptionName)
        {
            var db = _context.Create(typeof(GlobalOptionsRepository));
            return await db.GlobalOptions.Where(x => x.OptionName == OptionName).FirstOrDefaultAsync();
        }

        public async Task UpdateOptions(GlobalOptions option)
        {
            var db = _context.Create(typeof(GlobalOptionsRepository));
            db.GlobalOptions.Update(option);
            await db.SaveChangesAsync();
        }

        public async Task AddOptions(GlobalOptions option)
        {
            var db = _context.Create(typeof(GlobalOptionsRepository));
            await db.GlobalOptions.AddAsync(option);
            await db.SaveChangesAsync();
        }

    }
}
