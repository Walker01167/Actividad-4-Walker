using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestAPIPrueba.Dtos;
using RestAPIPrueba.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestAPIPrueba.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ActividadUnidad4Context _context;
        private readonly IConfiguration _config;

        public UsuarioController(ActividadUnidad4Context context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar(RegistroDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("Email ya registrado.");

            var user = new Usuario
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Usuario registrado.");
        }

        [HttpPost("iniciarSesion")]
        public async Task<IActionResult> IniciarSesion(LoginDto dto)
        {
            var user = await _context.Usuarios.SingleOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Credenciales inválidas.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("kN#e8R*VZ9m!P7xq^W@d4G$Tfq2L!a1b");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
            }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { token = tokenHandler.WriteToken(token) });
        }

        [HttpGet("perfil")]
        [Authorize]
        public async Task<IActionResult> Perfil()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _context.Usuarios.FindAsync(userId);
            return Ok(new { user.Name, user.Email });
        }
    }

}
