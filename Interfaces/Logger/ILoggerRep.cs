using Core.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Logger
{
    public interface ILoggerRep
    {
        public Task AddLog(LoggerItem item);
        public Task AddLog(LoggerLoginItem item);
        public Task<IEnumerable<LoggerLoginItem>> GetLogsAuth();
    }
}
