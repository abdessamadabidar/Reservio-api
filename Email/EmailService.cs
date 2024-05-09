using AutoMapper.Internal;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Reservio.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailService(IOptions<EmailConfiguration> options)
        {
           this._emailConfiguration = options.Value;
        }


        public string PrepareEmailTemplate(string FirstName, string LastName, string url)
        {
            var template = File.ReadAllText("template/verification.html");
            template = template.Replace("{{firstname}}", FirstName);
            template = template.Replace("{{lastname}}", LastName);
            template = template.Replace("{{verify_link}}", url); 
            return template;
        }

        public void SendEmail(Mail mail)
        {

            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_emailConfiguration.Email);
            email.To.Add(MailboxAddress.Parse(mail.To));
            email.Subject = mail.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = mail.Body;
            email.Body = builder.ToMessageBody();


            using var smtp = new SmtpClient();
            smtp.Connect(_emailConfiguration.Host, _emailConfiguration.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailConfiguration.Email, _emailConfiguration.Password);
            smtp.Send(email);
            smtp.Disconnect(true);




        }
    }
}
