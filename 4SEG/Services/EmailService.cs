namespace _4SEG.Services;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

public class EmailService : IEmailService
{
    public void EnviarEmail(string destinatario, string assunto, string mensagem)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Admin", "admin@example.com"));
        emailMessage.To.Add(new MailboxAddress("", destinatario));
        emailMessage.Subject = assunto;
        emailMessage.Body = new TextPart("plain") { Text = mensagem };

        using (var client = new SmtpClient())
        {
            client.Connect("smtp.example.com", 587, false);
            client.Authenticate("username", "password");
            client.Send(emailMessage);
            client.Disconnect(true);
        }
    }

    public Task EnviarEmailAsync(string destinatario, string assunto, string corpo)
    {
        throw new NotImplementedException();
    }
}
