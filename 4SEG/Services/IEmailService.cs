namespace _4SEG.Services
{
    using System.Threading.Tasks;

    public interface IEmailService
    {
        Task EnviarEmailAsync(string destinatario, string assunto, string corpo);
    }

}
