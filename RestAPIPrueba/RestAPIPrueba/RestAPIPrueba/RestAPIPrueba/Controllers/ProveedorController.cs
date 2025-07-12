using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPIPrueba.Models;
using System.Text.RegularExpressions;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProveedorController : ControllerBase
{
    private readonly ActividadUnidad4Context _context;

    public ProveedorController(ActividadUnidad4Context context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var proveedores = await _context.Set<Proveedor>().ToListAsync();
        return Ok(proveedores);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var proveedor = await _context.Set<Proveedor>().FindAsync(id);
        if (proveedor == null)
            return NotFound();

        return Ok(proveedor);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] Proveedor proveedor)
    {
        if (!ValidarProveedor(proveedor, out var errores))
            return BadRequest(errores);

        _context.Set<Proveedor>().Add(proveedor);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ObtenerPorId), new { id = proveedor.Id }, proveedor);
    }

    [HttpPut]
    public async Task<IActionResult> Actualizar([FromBody] Proveedor proveedor)
    {
        if (!ValidarProveedor(proveedor, out var errores))
            return BadRequest(errores);

        var existe = await _context.Set<Proveedor>().AnyAsync(p => p.Id == proveedor.Id);
        if (!existe)
            return NotFound();

        _context.Entry(proveedor).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Set<Proveedor>().AnyAsync(p => p.Id == proveedor.Id))
                return NotFound();

            throw;
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var proveedor = await _context.Set<Proveedor>().FindAsync(id);
        if (proveedor == null)
            return NotFound();

        _context.Set<Proveedor>().Remove(proveedor);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ValidarProveedor(Proveedor proveedor, out List<string> errores)
    {
        errores = new List<string>();

        if (string.IsNullOrWhiteSpace(proveedor.NombreProveedor) || proveedor.NombreProveedor.Length < 3)
            errores.Add("El nombre debe de tener alguna letra que lo identifique.");

        if (!EsTelefonoValido(proveedor.Telefono))
            errores.Add("ingresa un correo valido.");

        if (!EsCorreoValido(proveedor.Correo))
            errores.Add("ingresa un correo valido.");

        return errores.Count == 0;
    }

    private bool EsTelefonoValido(string telefono)
    {
        if (string.IsNullOrWhiteSpace(telefono))
            return false;

        var regex = new Regex(@"^[\d\s\-\+\(\)]{7,}$");
        return regex.IsMatch(telefono);
    }

    private bool EsCorreoValido(string correo)
    {
        if (string.IsNullOrWhiteSpace(correo))
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(correo);
            return addr.Address == correo;
        }
        catch
        {
            return false;
        }
    }
}
