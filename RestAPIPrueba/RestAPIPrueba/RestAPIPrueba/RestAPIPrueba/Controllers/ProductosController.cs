using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPIPrueba.Models;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductosController : ControllerBase
{
    private readonly ActividadUnidad4Context _context;

    public ProductosController(ActividadUnidad4Context context)
    {
        _context = context;
    }

    // GET: /api/productos
    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var productos = await _context.Productos.ToListAsync();
        return Ok(productos);
    }

    // GET: /api/productos/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorID(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null)
            return NotFound();

        return Ok(producto);
    }

    // POST: /api/productos
    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] Producto producto)
    {
        if (!ValidarProducto(producto, out var errores))
            return BadRequest(errores);

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ObtenerPorID), new { id = producto.Id }, producto);
    }

    // PUT: /api/productos/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] Producto producto)
    {
        if (id != producto.Id)
            return BadRequest("ID no coincide.");

        if (!ValidarProducto(producto, out var errores))
            return BadRequest(errores);

        _context.Entry(producto).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Productos.Any(p => p.Id == id))
                return NotFound();

            throw;
        }
    }

    // DELETE: /api/productos/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null)
            return NotFound();

        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool ValidarProducto(Producto producto, out List<string> errores)
    {
        errores = new List<string>();

        if (string.IsNullOrWhiteSpace(producto.Name) || producto.Name.Length < 3 || producto.Name.Length > 100)
            errores.Add("El nombre debe tener entre 3 y 100 caracteres.");

        if (producto.Price <= 0)
            errores.Add("El precio debe ser mayor a 0.");

        if (producto.Stock < 0)
            errores.Add("El stock no puede ser negativo.");

        return errores.Count == 0;
    }
}
