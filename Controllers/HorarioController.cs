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
    public class HorarioController : Controller
    {
        private readonly DataContext _context;

        public HorarioController(DataContext context)
        {
            _context = context;
        }

        #region GET
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Horario>>> GetAllHorarios()
        {
            try
            {
                var horarios = await _context.Horarios.ToListAsync();
                if (horarios == null || horarios.Count == 0)
                    return NotFound();

                return Ok(horarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region GET {id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Horario>> GetHorarioPorId(int id)
        {
            try
            {
                var horario = await _context.Horarios.FindAsync(id);
                if (horario == null)
                    return NotFound();

                return Ok(horario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region POST
        [HttpPost("AddHorario")]
        public async Task<ActionResult> PostHorario([FromBody] HorarioCreateDTO dto)
        {
            try
            {
                if (await _context.Horarios.AnyAsync(h => h.IdPosologia == dto.IdPosologia && h.Hora == dto.Hora))
                {
                    return BadRequest($"O horario {dto.Hora} da posologia {dto.IdPosologia} informada já existe.");
                }

                var posologia = await _context.Posologias.FirstOrDefaultAsync(p => p.IdPosologia == dto.IdPosologia);

                var h = new Horario();
                h.IdPosologia = dto.IdPosologia;
                h.Hora = dto.Hora;
                h.Posologia = posologia;

                _context.Horarios.Add(h);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetHorarioPorId), new { id = h.IdPosologia, h.Hora }, h);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar item: {ex.Message}");
            }
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public async Task<ActionResult> PutHorario(int id, [FromBody] HorarioUpdateDTO dto)
        {
            try
            {
                if (!await _context.Horarios.AnyAsync(h => h.IdPosologia == dto.IdPosologia && h.Hora == dto.Hora))
                {
                    return BadRequest($"O horario {dto.Hora} da posologia {dto.IdPosologia} informada não existe.");
                }
                var posologia = await _context.Posologias.FirstOrDefaultAsync(p => p.IdPosologia == dto.IdPosologia);

                // Atualize os campos
                var h = new Horario();
                h.IdPosologia = dto.IdPosologia;
                h.Hora = dto.Hora;
                h.Posologia = posologia;

                _context.Horarios.Update(h);
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
        [HttpDelete("{id}/{hora}")]
        public async Task<ActionResult> DeleteHorario(int id, TimeOnly hora)
        {
            try
            {
                var horario = await _context.Horarios.FirstOrDefaultAsync(x => x.IdPosologia == id && x.Hora == hora);
                if (horario == null)
                    return NotFound($"O Horario com o ID {id} não foi encontrado.");

                _context.Horarios.Remove(horario);
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
