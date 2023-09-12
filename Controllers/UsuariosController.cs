using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partyholic_api.Models;
using partyholic_api.Dto;
using System.Web.Http.Description;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace partyholic_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsuariosController : ControllerBase
    {
        private readonly partyholicContext _context;

        private readonly IConfiguration _configuration;

        private string CreateToken(Usuario usuario)
        {
            

        List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Username)
            };

            var tokenKey = _configuration.GetSection("AppSettings:TokenKey").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));


            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public UsuariosController(partyholicContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            if (_context.Usuarios == null)
            {
                return NotFound();
            }
            return await _context.Usuarios.ToListAsync();
        }

        // DTO  POST: api/Usuarios/auth/signin/username
        //@sgallego
        [Route("auth/signin")]
        [HttpPost]
        public IActionResult signin(Usuario usuario)
        {
            try
            {
                List<UsuarioAuthDto> LstUsuAuth = new List<UsuarioAuthDto>();
                foreach (Usuario prd in _context.Usuarios.ToList())
                {
                    if (usuario.Username == prd.Username && usuario.Passwd == prd.Passwd)
                    {
                        string token = CreateToken(usuario);

                        return Ok(new {accessToken = token, infoUser = prd});

                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        // DTO POST
        [Route("auth/signup")]
        [HttpPost]
        public IActionResult signup(Usuario usuario)
        {
            try
            {
                Usuario user = new Usuario();
                user.Username = usuario.Username;
                user.Email=usuario.Email;
                user.Passwd= usuario.Passwd;
                user.RolApp = "user";
                user.Nombre = usuario.Username;
                user.Privacidad = "public";
                _context.Usuarios.Add(user);
                _context.SaveChanges();
                return Ok(new { message = "Success" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(string id)
        {
            if (_context.Usuarios == null)
            {
                return NotFound();
            }
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(string id, Usuario usuario)
        {
            if (id != usuario.Username)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'PartyholicContext.Usuarios'  is null.");
            }
            _context.Usuarios.Add(usuario);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UsuarioExists(usuario.Username))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUsuario", new { id = usuario.Username }, usuario);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(string id)
        {
            if (_context.Usuarios == null)
            {
                return NotFound();
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(string id)
        {
            return (_context.Usuarios?.Any(e => e.Username == id)).GetValueOrDefault();
        }
    }
}
