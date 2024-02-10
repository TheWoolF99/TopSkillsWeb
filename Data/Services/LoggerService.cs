
using Core.Logger;
using Interfaces.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class LoggerService
    {
        private readonly ILoggerRep _log;

        public LoggerService(ILoggerRep logger) => this._log = logger;

        public async Task AddLog(LoggerItem item)
        {
            await _log.AddLog(item);
        }



    }
}
