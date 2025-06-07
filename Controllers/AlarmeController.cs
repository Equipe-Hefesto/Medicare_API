using System.Runtime.InteropServices;
using System.Security.Claims;
using Medicare_API.Data;
using Medicare_API.Models;
using Medicare_API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medicare_API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class AlarmeController : Controller
    {
        private readonly DataContext _context;

        public AlarmeController(DataContext context)
        {
            _context = context;
        }

        #region GET
        [HttpGet("GetAll")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<Alarme>>> GetAllAlarmes()
        {
            try
            {
                var alarmes = await _context.Alarmes.ToListAsync();
                if (alarmes == null || alarmes.Count == 0)
                    return NotFound();

                return Ok(alarmes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region GET {id}
        [HttpGet("utilizador")]
        public async Task<ActionResult<IEnumerable<Alarme>>> GetAlarmePorId()
        {
            try
            {

                var userString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userString, out int userId))
                {
                    return Unauthorized();
                }

                var alarme = await _context.Alarmes.Include(p => p.Posologia)
                .Where(p => p.Posologia.IdUtilizador == userId)
                .ToListAsync();

                //var alarme = await _context.Alarmes.FindAsync(id);
                if (alarme == null)
                    return NotFound();

                return Ok(alarme);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region POST
        [HttpPost]
        public async Task<ActionResult> PostAlarme([FromBody] AlarmeCreateDTO dto)
        {
            try
            {
                if (await _context.Alarmes.AnyAsync(a => a.IdPosologia == dto.IdPosologia && a.DataHora == dto.DataHora))
                {
                    return BadRequest($"O Alarme da Posologia  {dto.IdPosologia} das {dto.DataHora} informado já existe.");
                }

                var userString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userString, out int userId))
                {
                    return Unauthorized();
                }

                var posologia = await _context.Posologias.FirstOrDefaultAsync(p => p.IdPosologia == dto.IdPosologia && p.IdUtilizador == userId);

                if (posologia == null)
                {
                    return Forbid("Você não tem permissão para adicionar alarmes a essa posologia.");
                }

                var ultimoId = await _context.Alarmes.OrderByDescending(x => x.IdAlarme).Select(x => x.IdAlarme).FirstOrDefaultAsync();

                var a = new Alarme();
                a.IdAlarme = ultimoId + 1;
                a.IdPosologia = dto.IdPosologia;
                a.Descricao = dto.Descricao;
                a.DataHora = dto.DataHora;
                a.Status = "A";
                a.Posologia = posologia;

                _context.Alarmes.Add(a);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAlarmePorId), new { id = a.IdAlarme }, a);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar item: {ex.Message}");
            }
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
       
        public async Task<ActionResult> PutAlarme(int id, [FromBody] AlarmeUpdateDTO dto)
        {
            try
            {

                var userString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if(!int.TryParse(userString, out int userId))

                if (!await _context.Alarmes.AnyAsync(a => a.IdPosologia == dto.IdPosologia && a.DataHora == dto.DataHora))
                {
                    return BadRequest($"O Alarme da Posologia {dto.IdPosologia} das {dto.DataHora} informado já existe.");
                }

                var posologia = await _context.Posologias.FirstOrDefaultAsync(p => p.IdPosologia == dto.IdPosologia && p.IdUtilizador == userId);

                var a = new Alarme();
                a.IdAlarme = dto.IdAlarme;
                a.IdPosologia = dto.IdPosologia;
                a.Descricao = dto.Descricao;
                a.DataHora = dto.DataHora;
                a.Status = "A";
                a.Posologia = posologia;

                _context.Alarmes.Update(a);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar item: {ex.Message}");
            }
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        [Authorize(Roles = "AMIGO_MEDICARE")]
        public async Task<ActionResult> DeleteAlarme(int id)
        {
            try
            {
                var alarme = await _context.Alarmes.FirstOrDefaultAsync(x => x.IdAlarme == id);
                if (alarme == null)
                    return NotFound($"O Alarme com o ID {id} não foi encontrado.");

                _context.Alarmes.Remove(alarme);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar item: {ex.Message}");
            }
        }
        #endregion
    }
}