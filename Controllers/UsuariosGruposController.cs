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
    public class UsuariosGruposController : ControllerBase
    {
        private readonly partyholicContext _context;

        public UsuariosGruposController(partyholicContext context)
        {
            _context = context;
        }

        // GET: api/UsuariosGrupoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuariosGrupo>>> GetUsuariosGrupos()
        {
          if (_context.UsuariosGrupos == null)
          {
              return NotFound();
          }
            return await _context.UsuariosGrupos.ToListAsync();
        }

        // GET: api/UsuariosGrupoes/getGruposUsuario/username
        [HttpGet("getGruposUsuario/{username}")]
        public async Task<ActionResult<IEnumerable<Grupo>>> GetGruposUsuario(string username)
        {
            var gruposUsuario = await _context.UsuariosGrupos
                .Include(ug => ug.CodGrupoNavigation)
                .Where(ug => ug.Username == username)
                .Select(ug => ug.CodGrupoNavigation)
                .ToListAsync();

            if (gruposUsuario == null || !gruposUsuario.Any())
            {
                return Ok();
            }

            return Ok(gruposUsuario);
        }


        // GET: api/UsuariosGrupoes/{cod_grupo}
        [HttpGet("CodGrupo/{cod_grupo}")]
        public async Task<ActionResult<IEnumerable<UsuariosGrupo>>> GetUsuariosGrupoGrupo(int cod_grupo)
        {

            var eventoGrupos = _context.UsuariosGrupos.Where(a => a.CodGrupo == cod_grupo);

            if (eventoGrupos.Count() == 0)
            {
                return NotFound();
            }

            return await eventoGrupos.ToListAsync();
        }
        //-----------------------

        // PUT: api/UsuariosGrupoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuariosGrupo(int id, UsuariosGrupo usuariosGrupo)
        {
            if (id != usuariosGrupo.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuariosGrupo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuariosGrupoExists(id))
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

        // POST: api/UsuariosGrupoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UsuariosGrupo>> PostUsuariosGrupo(UsuariosGrupo usuariosGrupo)
        {
          if (_context.UsuariosGrupos == null)
          {
              return Problem("Entity set 'PartyholicContext.UsuariosGrupos'  is null.");
          }
            _context.UsuariosGrupos.Add(usuariosGrupo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuariosGrupo", new { id = usuariosGrupo.Id }, usuariosGrupo);
        }

        // DELETE: api/UsuariosGrupoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuariosGrupo(int id)
        {
            if (_context.UsuariosGrupos == null)
            {
                return NotFound();
            }
            var usuariosGrupo = await _context.UsuariosGrupos.FindAsync(id);
            if (usuariosGrupo == null)
            {
                return NotFound();
            }

            _context.UsuariosGrupos.Remove(usuariosGrupo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuariosGrupoExists(int id)
        {
            return (_context.UsuariosGrupos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
