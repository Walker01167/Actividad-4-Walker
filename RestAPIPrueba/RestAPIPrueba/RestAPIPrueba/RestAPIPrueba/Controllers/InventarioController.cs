using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPIPrueba.Models;

namespace RestAPIPrueba.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InventarioController : ControllerBase
    {
        private readonly ActividadUnidad4Context _context;

        public InventarioController(ActividadUnidad4Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var inventarios = await _context.Inventarios.ToListAsync();
            return Ok(inventarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var inventario = await _context.Inventarios.FindAsync(id);
            if (inventario == null)
                return NotFound();
            return Ok(inventario);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Inventario inventario)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productoExiste = await _context.Productos.AnyAsync(p => p.Id == inventario.ProductoId);
            if (!productoExiste)
                return BadRequest("por favor revisar.");

            _context.Inventarios.Add(inventario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Obtener), new { id = inventario.Id }, inventario);
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar(Inventario inventario)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existe = await _context.Inventarios.AnyAsync(i => i.Id == inventario.Id);
            if (!existe)
                return NotFound();

            var productoExiste = await _context.Productos.AnyAsync(p => p.Id == inventario.ProductoId);
            if (!productoExiste)
                return BadRequest("producto sin existencia.");

            _context.Inventarios.Update(inventario);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var inventario = await _context.Inventarios.FindAsync(id);
            if (inventario == null)
                return NotFound();

            _context.Inventarios.Remove(inventario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}