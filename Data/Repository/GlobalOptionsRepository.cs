using Core;
using Core.Mailer;
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

        public async Task UpdateOptions(string OptionsName, string OptionValue)
        {
            var db = _context.Create(typeof(GlobalOptionsRepository));
            var opt = db.GlobalOptions.Where(x => x.OptionName.ToLower() == OptionsName.ToLower()).FirstOrDefault();
            if(opt != null)
            {
                opt.OptionValue = OptionValue;
                await db.SaveChangesAsync();
            }
        }


        public async Task<MailOption> GetMailOptionAsync()
        {
            List<string> listField = new() {"MailTo",
                "MailToName",
                "From",
                "FromName",
                "SMTPHost",
                "SMTPPort",
                "SMTPUseSSL",
                "SMTPLogin",
                "SMTPPassword"
            };

            var db = _context.Create(typeof(GlobalOptionsRepository));
            var MailOption = db.GlobalOptions.Where(x => listField.Contains(x.OptionName)).ToDictionary(x=>x.OptionName, x=>x.OptionValue);
            MailOption opt = new()
            {
                From = MailOption["From"],
                FromName = MailOption["FromName"],
                To = MailOption["MailTo"],
                ToName = MailOption["MailToName"],
                SMTPHost = MailOption["SMTPHost"],
                SMTPPort = Convert.ToInt32(MailOption["SMTPPort"]),
                SMTPUseSSL = Convert.ToBoolean(MailOption["SMTPUseSSL"]),
                SMTPLogin = MailOption["SMTPLogin"],
                SMTPPassword = MailOption["SMTPPassword"]
            };
            return opt;
        }


    }
}
