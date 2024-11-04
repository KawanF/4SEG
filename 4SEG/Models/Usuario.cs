namespace _4SEG.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Senha { get; set; }  // Armazenada em SHA256
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Perfil { get; set; }
        public string IPAutorizado { get; set; }
    }

}
