using _4SEG.Data;
using _4SEG.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AlunoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AlunoController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_context.Alunos.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var aluno = _context.Alunos.Find(id);
        if (aluno == null) return NotFound();
        return Ok(aluno);
    }

    [HttpPost]
    public IActionResult Create(Aluno aluno)
    {
        _context.Alunos.Add(aluno);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = aluno.Id }, aluno);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Aluno aluno)
    {
        var alunoExistente = _context.Alunos.Find(id);
        if (alunoExistente == null) return NotFound();

        alunoExistente.Nome = aluno.Nome;
        alunoExistente.Idade = aluno.Idade;
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var aluno = _context.Alunos.Find(id);
        if (aluno == null) return NotFound();

        _context.Alunos.Remove(aluno);
        _context.SaveChanges();
        return NoContent();
    }
}
