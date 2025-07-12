using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPIPrueba.Models;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriaController : ControllerBase
{
    private readonly ActividadUnidad4Context _context;

    public CategoriaController(ActividadUnidad4Context context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var categorias = await _context.Set<Categoria>().ToListAsync();
        return Ok(categorias);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var categoria = await _context.Set<Categoria>().FindAsync(id);
        if (categoria == null)
            return NotFound();

        return Ok(categoria);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] Categoria categoria)
    {
        if (!ValidarCategoria(categoria, out var errores))
            return BadRequest(errores);

        _context.Set<Categoria>().Add(categoria);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ObtenerPorId), new { id = categoria.Id }, categoria);
    }

    [HttpPut]
    public async Task<IActionResult> Actualizar([FromBody] Categoria categoria)
    {
        if (!ValidarCategoria(categoria, out var errores))
            return BadRequest(errores);

        var existe = await _context.Set<Categoria>().AnyAsync(c => c.Id == categoria.Id);
        if (!existe)
            return NotFound();

        _context.Entry(categoria).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Set<Categoria>().AnyAsync(c => c.Id == categoria.Id))
                return NotFound();

            throw;
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var categoria = await _context.Set<Categoria>().FindAsync(id);
        if (categoria == null)
            return NotFound();

        _context.Set<Categoria>().Remove(categoria);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ValidarCategoria(Categoria categoria, out List<string> errores)
    {
        errores = new List<string>();

        if (string.IsNullOrWhiteSpace(categoria.Name) || categoria.Name.Length < 5)
            errores.Add("La categoria debe tener alguna letra.");

        return errores.Count == 0;
    }
}