using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IGlobalOptions
    {
        Task<IEnumerable<GlobalOptions>> GetAllOptions(string? search);
        Task<GlobalOptions?> GetOptionsByName(string OptionName);
        Task UpdateOptions(GlobalOptions option);
        Task AddOptions(GlobalOptions option);
    }
}
