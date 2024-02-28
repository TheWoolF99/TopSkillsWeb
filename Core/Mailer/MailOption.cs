using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mailer
{
    public class MailOption
    {
        //Данные о письме
        public string From { get; set; }
        public string FromName { get; set; } = "Administation";

        public string To { get; set; }
        public string ToName { get; set; } = "You";

        //Данные для подключения к SMTP
        public string SMTPHost { get; set; }
        public int SMTPPort { get; set; }
        public bool SMTPUseSSL { get; set; }

        //Данные для аутентификации в SMTP
        public string SMTPLogin { get; set; }
        public string SMTPPassword { get; set; }
    }
}
