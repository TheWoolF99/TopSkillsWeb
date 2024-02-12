using Core;
using Interfaces;
using Interfaces.Abonement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class GlobalOptionsService
    {
        private readonly IGlobalOptions _options;
        public GlobalOptionsService(IGlobalOptions options) => this._options = options;

        public async Task<IEnumerable<GlobalOptions>> GetAllOptions(string? search)
        {
            return await _options.GetAllOptions(search);
        }

        public async Task<GlobalOptions?> GetOptionsByName(string OptionName)
        {
            return await _options.GetOptionsByName(OptionName);
        }

        public async Task UpdateOptions(GlobalOptions option)
        { 
            await _options.UpdateOptions(option);
        }

        public async Task AddOptions(GlobalOptions option)
        {
            await _options.AddOptions(option);
        }
    }
}

