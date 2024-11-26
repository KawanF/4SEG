using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace _4SEG.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _fromEmail;
        private readonly string _password;

        public EmailService(IConfiguration configuration)
        {
            _smtpServer = configuration["EmailConfig:SmtpServer"];
            _smtpPort = int.Parse(configuration["EmailConfig:SmtpPort"]);
            _fromEmail = configuration["EmailConfig:FromEmail"];
            _password = configuration["EmailConfig:Password"];
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            var message = new MailMessage(_fromEmail, toEmail, subject, body);
            var client = new SmtpClient
            {
                Host = _smtpServer,
                Port = _smtpPort,
                EnableSsl = true,
                Credentials = new NetworkCredential(_fromEmail, _password)
            };
            client.Send(message);
        }
    }
}