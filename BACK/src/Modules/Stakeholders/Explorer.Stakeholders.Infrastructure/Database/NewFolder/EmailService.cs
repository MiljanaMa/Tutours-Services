using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Explorer.Stakeholders.API.Internal;
using Explorer.Stakeholders.Core.UseCases;

namespace Explorer.Stakeholders.Infrastructure.Database.NewFolder
{
    public class EmailService : IInternalEmailService, IEmailVerificationService
    {
        public EmailService()
        {

        }
        public void SendEmail(string toMail, string subject, string body)
        {
            string fromMail = "psw.team8@gmail.com";
            string fromPassword = "qkgtrdohfzigxxti";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(toMail));
            message.Body = body;
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);
        }
    }
}
