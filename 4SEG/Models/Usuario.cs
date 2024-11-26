namespace _4SEG.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Senha { get; set; }  
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Perfil { get; set; }
        public string IPAutorizado { get; set; }

        public string PasswordRecoveryToken { get; set; }
        public DateTime? PasswordRecoveryTokenExpiration { get; set; }

        public string OtpCode { get; set; }
        public DateTime? OtpExpiration { get; set; }
    }

}
