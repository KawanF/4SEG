namespace _4SEG.Models
{
    public class Professor
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Disciplina { get; set; } 
        public ICollection<Nota> Notas { get; set; } 
    }

}