using _4SEG.Data;
using _4SEG.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProfessorController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProfessorController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_context.Professores.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var professor = _context.Professores.Find(id);
        if (professor == null) return NotFound();
        return Ok(professor);
    }

    [HttpPost]
    public IActionResult Create(Professor professor)
    {
        _context.Professores.Add(professor);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = professor.Id }, professor);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Professor professor)
    {
        var professorExistente = _context.Professores.Find(id);
        if (professorExistente == null) return NotFound();

        professorExistente.Nome = professor.Nome;
        professorExistente.Disciplina = professor.Disciplina;
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var professor = _context.Professores.Find(id);
        if (professor == null) return NotFound();

        _context.Professores.Remove(professor);
        _context.SaveChanges();
        return NoContent();
    }
}