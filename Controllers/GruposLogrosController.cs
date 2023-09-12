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
    public class GruposLogrosController : ControllerBase
    {
        private readonly partyholicContext _context;

        public GruposLogrosController(partyholicContext context)
        {
            _context = context;
        }

        // GET: api/GruposLogroes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GruposLogro>>> GetGruposLogros()
        {
          if (_context.GruposLogros == null)
          {
              return NotFound();
          }
            return await _context.GruposLogros.ToListAsync();
        }

        // GET: api/GruposLogroes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GruposLogro>> GetGruposLogro(int id)
        {
          if (_context.GruposLogros == null)
          {
              return NotFound();
          }
            var gruposLogro = await _context.GruposLogros.FindAsync(id);

            if (gruposLogro == null)
            {
                return NotFound();
            }

            return gruposLogro;
        }

        // GET: api/GruposLogroes/{cod_grupo}
        [HttpGet("CodGrupo/{cod_grupo}")]
        public async Task<ActionResult<IEnumerable<GruposLogro>>> GetGrpLogro(int cod_grupo)
        {

            var logrosGrp = _context.GruposLogros.Where(a => a.CodGrupo == cod_grupo);

            if (logrosGrp.Count() == 0)
            {
                return NotFound();
            }

            return await logrosGrp.ToListAsync();
        }

        // PUT: api/GruposLogroes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGruposLogro(int id, GruposLogro gruposLogro)
        {
            if (id != gruposLogro.Id)
            {
                return BadRequest();
            }

            _context.Entry(gruposLogro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GruposLogroExists(id))
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

        // POST: api/GruposLogroes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GruposLogro>> PostGruposLogro(GruposLogro gruposLogro)
        {
          if (_context.GruposLogros == null)
          {
              return Problem("Entity set 'PartyholicContext.GruposLogros'  is null.");
          }
            _context.GruposLogros.Add(gruposLogro);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGruposLogro", new { id = gruposLogro.Id }, gruposLogro);
        }

        // DELETE: api/GruposLogroes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGruposLogro(int id)
        {
            if (_context.GruposLogros == null)
            {
                return NotFound();
            }
            var gruposLogro = await _context.GruposLogros.FindAsync(id);
            if (gruposLogro == null)
            {
                return NotFound();
            }

            _context.GruposLogros.Remove(gruposLogro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GruposLogroExists(int id)
        {
            return (_context.GruposLogros?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
