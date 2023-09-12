using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partyholic_api.Models;

namespace partyholic_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MensajesController : ControllerBase
    {
        private readonly partyholicContext _context;

        public MensajesController(partyholicContext context)
        {
            _context = context;
        }

        // GET: api/Mensajes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mensaje>>> GetMensajes()
        {
          if (_context.Mensajes == null)
          {
              return NotFound();
          }
            return await _context.Mensajes.ToListAsync();
        }

        // GET: api/Mensajes/{cod_grupo}
        [HttpGet("CodGrupo/{cod_grupo}")]
        public async Task<ActionResult<IEnumerable<Mensaje>>> GetMensajesGrupo(int cod_grupo)
        {

            var msjGrupo = _context.Mensajes.Where(a => a.CodGrupo == cod_grupo);

            if (msjGrupo.Count() == 0)
            {
                return NotFound();
            }

            return await msjGrupo.ToListAsync();
        }

        [Route("crear")]
        [HttpPost]
        public IActionResult signup(Mensaje m)
        {
            try
            {
                Mensaje mensaje = new Mensaje();
                mensaje.CodGrupo = m.CodGrupo;
                mensaje.Username = m.Username;
                mensaje.Contenido = m.Contenido;
                _context.Mensajes.Add(mensaje);
                _context.SaveChanges();
                return Ok(new { message = "Success" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        // GET: api/Mensajes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Mensaje>> GetMensaje(int id)
        {
          if (_context.Mensajes == null)
          {
              return NotFound();
          }
            var mensaje = await _context.Mensajes.FindAsync(id);

            if (mensaje == null)
            {
                return NotFound();
            }

            return mensaje;
        }

        // PUT: api/Mensajes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMensaje(int id, Mensaje mensaje)
        {
            if (id != mensaje.IdMensaje)
            {
                return BadRequest();
            }

            _context.Entry(mensaje).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MensajeExists(id))
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

        // POST: api/Mensajes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Mensaje>> PostMensaje(Mensaje mensaje)
        {
          if (_context.Mensajes == null)
          {
              return Problem("Entity set 'PartyholicContext.Mensajes'  is null.");
          }
            _context.Mensajes.Add(mensaje);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMensaje", new { id = mensaje.IdMensaje }, mensaje);
        }

        // DELETE: api/Mensajes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMensaje(int id)
        {
            if (_context.Mensajes == null)
            {
                return NotFound();
            }
            var mensaje = await _context.Mensajes.FindAsync(id);
            if (mensaje == null)
            {
                return NotFound();
            }

            _context.Mensajes.Remove(mensaje);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MensajeExists(int id)
        {
            return (_context.Mensajes?.Any(e => e.IdMensaje == id)).GetValueOrDefault();
        }
    }
}
