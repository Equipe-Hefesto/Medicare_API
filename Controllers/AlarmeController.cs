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
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Authorize(Roles = "AMIGO_MEDICARE")]
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
        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN")]
        [Authorize(Roles = "AMIGO_MEDICARE")]
        public async Task<ActionResult<Alarme>> GetAlarmePorId(int id)
        {
            try
            {
                var alarme = await _context.Alarmes.FindAsync(id);
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
        [Authorize(Roles = "ADMIN")]
        [Authorize(Roles = "AMIGO_MEDICARE")]
        public async Task<ActionResult> PostAlarme([FromBody] AlarmeCreateDTO dto)
        {
            try
            {
                if (await _context.Alarmes.AnyAsync(a => a.IdPosologia == dto.IdPosologia && a.DataHora == dto.DataHora))
                {
                    return BadRequest($"O Alarme da Posologia  {dto.IdPosologia} das {dto.DataHora} informado já existe.");
                }

                var posologia = await _context.Posologias.FirstOrDefaultAsync(p => p.IdPosologia == dto.IdPosologia);

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
        [Authorize(Roles = "ADMIN")]
        [Authorize(Roles = "AMIGO_MEDICARE")]
        public async Task<ActionResult> PutAlarme(int id, [FromBody] AlarmeUpdateDTO dto)
        {
            try
            {
                if (!await _context.Alarmes.AnyAsync(a => a.IdPosologia == dto.IdPosologia && a.DataHora == dto.DataHora))
                {
                    return BadRequest($"O Alarme da Posologia  {dto.IdPosologia} das {dto.DataHora} informado já existe.");
                }

                var posologia = await _context.Posologias.FirstOrDefaultAsync(p => p.IdPosologia == dto.IdPosologia);

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