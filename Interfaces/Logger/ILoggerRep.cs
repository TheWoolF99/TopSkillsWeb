using Core.Logger;

namespace Interfaces.Logger
{
    public interface ILoggerRep
    {
        public Task AddLog(LoggerItem item);

        public Task AddLog(LoggerLoginItem item);

        public Task<IEnumerable<LoggerLoginItem>> GetLogsAuth();
    }
}