namespace _4SEG.Services
{
    using System.Threading.Tasks;

    public interface IEmailService
    {
        void SendEmail(string toEmail, string subject, string body);
    }

}
