namespace _4SEG.Models
{
    public class Nota
    {
        public int Id { get; set; }
        public int AlunoId { get; set; } 
        public int ProfessorId { get; set; } 
        public double Valor { get; set; } 
        public DateTime Data { get; set; } 

        public required Aluno Aluno { get; set; } 
        public required Professor Professor { get; set; } 
    }

}