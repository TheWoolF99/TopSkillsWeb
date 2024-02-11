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
        public MailerController(AbonementService abonement) => this._abonement = abonement;

        public async Task<JsonResult> GetExpiredSubscription()
        {
            var StudentsAbonement = (await _abonement.GetAllAbonements())?.Where(x=>x.RemainingVisits <= 0).ToList();

            var data = new JsonResult(new { result = "" });

            if (StudentsAbonement.Count() > 0)
            {
                var m = new MimeMessage();
                m.From.Add(new MailboxAddress("I", "kuznetzov.max-on@yandex.ru"));

                m.To.Add(new MailboxAddress("You", "​topskills43@yandex.ru"));
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
                        client.Connect("smtp.yandex.ru", 465, true);
                        client.Authenticate("kuznetzov.max-on@yandex.ru", "ajkhazwfwjxfbdnc");
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

    }
}
