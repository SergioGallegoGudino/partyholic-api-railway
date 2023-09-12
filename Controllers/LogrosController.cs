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
    public class LogrosController : ControllerBase
    {
        private readonly partyholicContext _context;

        public LogrosController(partyholicContext context)
        {
            _context = context;
        }

        // GET: api/Logroes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Logro>>> GetLogros()
        {
          if (_context.Logros == null)
          {
              return NotFound();
          }
            return await _context.Logros.ToListAsync();
        }

        // GET: api/Logroes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Logro>> GetLogro(int id)
        {
          if (_context.Logros == null)
          {
              return NotFound();
          }
            var logro = await _context.Logros.FindAsync(id);

            if (logro == null)
            {
                return NotFound();
            }

            return logro;
        }

        // PUT: api/Logroes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLogro(int id, Logro logro)
        {
            if (id != logro.CodLogro)
            {
                return BadRequest();
            }

            _context.Entry(logro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogroExists(id))
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

        // POST: api/Logroes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Logro>> PostLogro(Logro logro)
        {
          if (_context.Logros == null)
          {
              return Problem("Entity set 'PartyholicContext.Logros'  is null.");
          }
            _context.Logros.Add(logro);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLogro", new { id = logro.CodLogro }, logro);
        }

        // DELETE: api/Logroes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLogro(int id)
        {
            if (_context.Logros == null)
            {
                return NotFound();
            }
            var logro = await _context.Logros.FindAsync(id);
            if (logro == null)
            {
                return NotFound();
            }

            _context.Logros.Remove(logro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LogroExists(int id)
        {
            return (_context.Logros?.Any(e => e.CodLogro == id)).GetValueOrDefault();
        }
    }
}
