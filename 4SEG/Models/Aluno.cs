namespace _4SEG.Models
{
    public class Aluno
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public string Matricula { get; set; } // Identificação do aluno, como matrícula
        public ICollection<Nota> Notas { get; set; } // Relacionamento com notas
    }

}