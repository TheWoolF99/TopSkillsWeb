using Core.Logger;
using Interfaces.Logger;
using NLog;

namespace Data.Services
{
    public class LoggerService
    {
        private readonly ILoggerRep _log;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public LoggerService(ILoggerRep logger) => this._log = logger;

        public async Task AddLog(LoggerItem item)
        {
            await _log.AddLog(item);
        }

        public void AddNLog(string Message)
        {
            logger.Info(Message);
        }

        public async Task AddLog(LoggerLoginItem item)
        {
            await _log.AddLog(item);
        }

        public async Task<IEnumerable<LoggerLoginItem>> GetLogsAuth()
        {
            return await _log.GetLogsAuth();
        }

        public async Task<IEnumerable<LoggerLoginItem>> GetLogsAuthWithFilter(LoggerFilter filters)
        {
            var list = await _log.GetLogsAuth();
            list = list.Where(x => x.Date.Date >= filters.DateStart.Date && x.Date.Date <= filters.DateEnd.Date).ToList();
            if (!String.IsNullOrEmpty(filters.UserName))
            {
                list = list.Where(x => x.UserName.ToLower() == filters.UserName.ToLower()).ToList();
            }
            return list;
        }
    }
}