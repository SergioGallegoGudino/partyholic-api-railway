using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partyholic_api.Models;

namespace partyholic_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly partyholicContext _context;

        public EventosController(partyholicContext context)
        {
            _context = context;
        }

        // GET: api/Eventoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventos()
        {
            if (_context.Eventos == null)
            {
                return NotFound();
            }
            return await _context.Eventos.ToListAsync();
        }
        // GET: api/Eventoes/{cod_grupo}
        [HttpGet("CodGrupo/{cod_grupo}")]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventoGrupo(int cod_grupo)
        {

            var eventoGrupos = _context.Eventos.Where(a => a.CodGrupo == cod_grupo);

            if (eventoGrupos.Count() == 0)
            {
                return NotFound();
            }

            return await eventoGrupos.ToListAsync();
        }


        // GET: api/Eventoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Evento>> GetEvento(int id)
        {
            if (_context.Eventos == null)
            {
                return NotFound();
            }
            var evento = await _context.Eventos.FindAsync(id);

            if (evento == null)
            {
                return NotFound();
            }

            return evento;
        }

        // PUT: api/Eventoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvento(int id, Evento evento)
        {
            if (id != evento.CodEvento)
            {
                return BadRequest();
            }

            _context.Entry(evento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventoExists(id))
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
        // POST api/Eventos
        [HttpPost]
        public IActionResult PostEvento(Evento evento)
        {

            try
            {
                Evento ev = new Evento();
                ev.CodEvento = evento.CodEvento;
                //evento.CodGrupo = 3;
                ev.CodGrupo = evento.CodGrupo;
                ev.Titulo = evento.Titulo;
                ev.FechaEvento = evento.FechaEvento;

                _context.Eventos.Add(ev);
                _context.SaveChanges();
                return Ok(new { message = "Success" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }


        //POST: api/Eventoes
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754        
        //[HttpPost]
        //public async Task<ActionResult<Evento>> PostEvento(Evento evento)
        //{
        //    if (_context.Eventos == null)
        //    {
        //        return Problem("Entity set 'PartyholicContext.Eventos'  is null.");
        //    }
        //    _context.Eventos.Add(evento);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (EventoExists(evento.CodEvento))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //    return CreatedAtAction("GetEvento", new { id = evento.CodEvento }, evento);
        //}


        // DELETE: api/Eventoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            if (_context.Eventos == null)
            {
                return NotFound();
            }
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventoExists(int id)
        {
            return (_context.Eventos?.Any(e => e.CodEvento == id)).GetValueOrDefault();
        }
    }
}
