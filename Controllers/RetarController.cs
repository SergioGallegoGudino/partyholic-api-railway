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
    public class RetarController : ControllerBase
    {
        private readonly partyholicContext _context;

        public RetarController(partyholicContext context)
        {
            _context = context;
        }

        // GET: api/Retars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Retar>>> GetRetars()
        {
          if (_context.Retars == null)
          {
              return NotFound();
          }
            return await _context.Retars.ToListAsync();
        }

        // GET: api/Retars/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Retar>> GetRetar(int id)
        {
          if (_context.Retars == null)
          {
              return NotFound();
          }
            var retar = await _context.Retars.FindAsync(id);

            if (retar == null)
            {
                return NotFound();
            }

            return retar;
        }

        // PUT: api/Retars/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRetar(int id, Retar retar)
        {
            if (id != retar.Id)
            {
                return BadRequest();
            }

            _context.Entry(retar).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RetarExists(id))
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

        // POST: api/Retars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Retar>> PostRetar(Retar retar)
        {
          if (_context.Retars == null)
          {
              return Problem("Entity set 'PartyholicContext.Retars'  is null.");
          }
            _context.Retars.Add(retar);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRetar", new { id = retar.Id }, retar);
        }

        // DELETE: api/Retars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRetar(int id)
        {
            if (_context.Retars == null)
            {
                return NotFound();
            }
            var retar = await _context.Retars.FindAsync(id);
            if (retar == null)
            {
                return NotFound();
            }

            _context.Retars.Remove(retar);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RetarExists(int id)
        {
            return (_context.Retars?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
