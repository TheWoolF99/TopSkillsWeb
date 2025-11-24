using Core.Logger;
using Interfaces.Logger;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class LoggerRepository : ILoggerRep
    {
        private DbContextFactory _context;

        public LoggerRepository(DbContextFactory context) => this._context = context;

        public async Task AddLog(LoggerItem item)
        {
            var db = _context.Create(typeof(LoggerRepository));
            await db.Logger.AddAsync(item);
            await db.SaveChangesAsync();
        }

        public async Task AddLog(LoggerLoginItem item)
        {
            var db = _context.Create(typeof(LoggerRepository));
            await db.LogAuth.AddAsync(item);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<LoggerLoginItem>> GetLogsAuth()
        {
            var db = _context.Create(typeof(LoggerRepository));
            return await db.LogAuth.ToListAsync();
        }
    }
}