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
    public class UsuariosEventosController : ControllerBase
    {
        private readonly partyholicContext _context;

        public UsuariosEventosController(partyholicContext context)
        {
            _context = context;
        }

        // GET: api/UsuariosEventoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuariosEvento>>> GetUsuariosEventos()
        {
          if (_context.UsuariosEventos == null)
          {
              return NotFound();
          }
            return await _context.UsuariosEventos.ToListAsync();
        }

        // GET: api/UsuariosEventoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuariosEvento>> GetUsuariosEvento(int id)
        {
          if (_context.UsuariosEventos == null)
          {
              return NotFound();
          }
            var usuariosEvento = await _context.UsuariosEventos.FindAsync(id);

            if (usuariosEvento == null)
            {
                return NotFound();
            }

            return usuariosEvento;
        }

        // PUT: api/UsuariosEventoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuariosEvento(int id, UsuariosEvento usuariosEvento)
        {
            if (id != usuariosEvento.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuariosEvento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuariosEventoExists(id))
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

        // POST: api/UsuariosEventoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UsuariosEvento>> PostUsuariosEvento(UsuariosEvento usuariosEvento)
        {
          if (_context.UsuariosEventos == null)
          {
              return Problem("Entity set 'PartyholicContext.UsuariosEventos'  is null.");
          }
            _context.UsuariosEventos.Add(usuariosEvento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuariosEvento", new { id = usuariosEvento.Id }, usuariosEvento);
        }

        // DELETE: api/UsuariosEventoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuariosEvento(int id)
        {
            if (_context.UsuariosEventos == null)
            {
                return NotFound();
            }
            var usuariosEvento = await _context.UsuariosEventos.FindAsync(id);
            if (usuariosEvento == null)
            {
                return NotFound();
            }

            _context.UsuariosEventos.Remove(usuariosEvento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuariosEventoExists(int id)
        {
            return (_context.UsuariosEventos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
