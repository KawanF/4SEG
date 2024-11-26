using _4SEG.Data;
using _4SEG.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotaController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public NotaController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_context.Notas.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var nota = _context.Notas.Find(id);
        if (nota == null) return NotFound();
        return Ok(nota);
    }

    [HttpPost]
    public IActionResult Create(Nota nota)
    {
        _context.Notas.Add(nota);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = nota.Id }, nota);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Nota nota)
    {
        var notaExistente = _context.Notas.Find(id);
        if (notaExistente == null) return NotFound();

        notaExistente.Valor = nota.Valor;
        notaExistente.Data = nota.Data;
        notaExistente.AlunoId = nota.AlunoId;
        notaExistente.ProfessorId = nota.ProfessorId;
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var nota = _context.Notas.Find(id);
        if (nota == null) return NotFound();

        _context.Notas.Remove(nota);
        _context.SaveChanges();
        return NoContent();
    }
}