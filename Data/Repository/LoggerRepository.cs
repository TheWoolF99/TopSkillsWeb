using Core.Logger;
using Interfaces.Attendance;
using Interfaces.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class LoggerRepository : ILoggerRep
    {
        private DbContextFactory _context;

        public LoggerRepository(DbContextFactory context)=>this._context=context;


        public async Task AddLog(LoggerItem item)
        {
            var db = _context.Create(typeof(LoggerRepository));
            await db.Logger.AddAsync(item);
            await db.SaveChangesAsync();
        }



    }
}
