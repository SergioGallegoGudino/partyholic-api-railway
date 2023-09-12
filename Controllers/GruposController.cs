using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partyholic_api.Models;

namespace partyholic_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GruposController : ControllerBase
    {
        private readonly partyholicContext _context;

        public GruposController(partyholicContext context)
        {
            _context = context;
        }

        [Route("crear")]
        [HttpPost]
        public IActionResult signup(Grupo g)
        {
            try
            {
                Grupo grupo = new Grupo();
                grupo.CodGrupo = g.CodGrupo;
                grupo.Nombre = g.Nombre;
                grupo.Privacidad = g.Privacidad;
                grupo.Participantes = g.Participantes;
                grupo.Descripcion = g.Descripcion;
                grupo.Juego = g.Juego;
                grupo.FotoGrupo = g.FotoGrupo;
                _context.Grupos.Add(grupo);
                _context.SaveChanges();
                return Ok(new { message = "Success" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        // GET: api/Grupoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Grupo>>> GetGrupos()
        {
          if (_context.Grupos == null)
          {
              return NotFound();
          }
            return await _context.Grupos.ToListAsync();
        }

        // GET: api/Grupoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Grupo>> GetGrupo(int id)
        {
          if (_context.Grupos == null)
          {
              return NotFound();
          }
            var grupo = await _context.Grupos.FindAsync(id);

            if (grupo == null)
            {
                return NotFound();
            }

            return grupo;
        }

        // PUT: api/Grupoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGrupo(int id, Grupo grupo)
        {
            if (id != grupo.CodGrupo)
            {
                return BadRequest();
            }

            _context.Entry(grupo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GrupoExists(id))
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


        // GET: api/UsuariosGrupoes/getGruposLike/input

        [HttpGet("getGruposLike/{input}")]
        public async Task<ActionResult<IEnumerable<Grupo>>> GetGruposLike(string input)
        {
            try
            {
                var grupos = await _context.Grupos
                    .Where(g => g.Nombre.Contains(input))
                    .ToListAsync();

                if (grupos == null || !grupos.Any())
                {
                    return await _context.Grupos.ToListAsync();
                }

                return Ok(grupos);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al buscar grupos: " + ex.ToString());
            }
        }

        // POST: api/Grupoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Grupo>> PostGrupo(Grupo grupo)
        {
          if (_context.Grupos == null)
          {
              return Problem("Entity set 'PartyholicContext.Grupos'  is null.");
          }
            _context.Grupos.Add(grupo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGrupo", new { id = grupo.CodGrupo }, grupo);
        }

        // DELETE: api/Grupoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrupo(int id)
        {
            if (_context.Grupos == null)
            {
                return NotFound();
            }
            var grupo = await _context.Grupos.FindAsync(id);
            if (grupo == null)
            {
                return NotFound();
            }

            _context.Grupos.Remove(grupo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GrupoExists(int id)
        {
            return (_context.Grupos?.Any(e => e.CodGrupo == id)).GetValueOrDefault();
        }
    }
}
