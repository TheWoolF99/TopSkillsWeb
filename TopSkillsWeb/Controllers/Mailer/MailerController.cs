using Data.Services;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Security.Cryptography;

namespace TopSkillsWeb.Controllers.Mailer
{
    public class MailerController : Controller
    {
        private readonly AbonementService _abonement;
        private readonly GlobalOptionsService _options;
        public MailerController(AbonementService abonement, GlobalOptionsService options)
        { 
            this._abonement = abonement; 
            this._options = options;
        }

        public async Task<JsonResult> GetExpiredSubscription()
        {
            var StudentsAbonement = (await _abonement.GetAllAbonements())?.Where(x=>x.RemainingVisits <= 0).ToList();
            var Options = await _options.GetMailOptionAsync();
            var data = new JsonResult(new { result = "" });
            if (StudentsAbonement.Count() > 0)
            {
                var m = new MimeMessage();
                m.From.Add(new MailboxAddress(Options.FromName, Options.From));
                m.To.Add(new MailboxAddress(Options.ToName, Options.To)); //topskills43@yandex.ru
                m.Subject ="Окончание абонементов!";
                m.Priority = MessagePriority.Urgent;
                string BodyStr = $"<p>Мы хотим Вас уведомить, что абонементы у следующих студентов истекли:</p>";

                BodyStr += "<ul>";
                foreach (var item in StudentsAbonement)
                {
                    BodyStr += $"<br><li>{item.Student.FullName}</li><br>";
                }
                BodyStr += "</ul>" +
                    "<br>" +
                    "Просим Вас связаться с ними и обновить их абонементы для продолжения доступа к нашим услугам." +
                    "\nС уважением," +
                    "\nАдминистрация";


                var build = new BodyBuilder();
                build.HtmlBody = String.Format(BodyStr);
                m.Body = build.ToMessageBody();

                

                using (var client = new SmtpClient())
                {
                    try
                    {
                        client.Connect(Options.SMTPHost, Options.SMTPPort, Options.SMTPUseSSL);
                        client.Authenticate(Options.SMTPLogin, Options.SMTPPassword);
                        client.Send(m);
                        client.Disconnect(true);
                        data.Value = new { result = "successfully", code = "200" };
                    }
                    catch (BadHttpRequestException ex)
                    {
                        client.Disconnect(true);
                        data.Value = new { result = ex.Message, code = ex.StatusCode };
                    }
                }
            }
            
            return data;
        }

        [Authorize(Roles = "OwnerApp")]
        public async Task<JsonResult> GetTestSendMail()
        {
            var Options = await _options.GetMailOptionAsync();
            var data = new JsonResult(new { result = "" });
            var m = new MimeMessage();
            m.From.Add(new MailboxAddress(Options.FromName, Options.From));
            m.To.Add(new MailboxAddress(Options.ToName, Options.To));
            m.Subject = $"Тестовое письмо! От {User.Identity.Name}";
            m.Priority = MessagePriority.Urgent;
            var build = new BodyBuilder();
            build.HtmlBody = String.Format("Тестовое письмо");
            m.Body = build.ToMessageBody();
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(Options.SMTPHost, Options.SMTPPort, Options.SMTPUseSSL);
                    client.Authenticate(Options.SMTPLogin, Options.SMTPPassword);
                    client.Send(m);
                    client.Disconnect(true);
                    data.Value = new { result = "successfully", code = "200" };
                }
                catch(Exception ex)
                {
                    client.Disconnect(true);
                    data.Value = new { result = ex.Message, code = "500" };
                    return data;
                }
            }

            return data;
        }

    }
}
