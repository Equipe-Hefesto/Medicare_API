using Medicare_API.Data;
using Medicare_API.Models;
using Medicare_API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medicare_API.Controllers
{
    [Route("[controller]")]
    public class TipoAgendamentoController : Controller
    {
        private readonly DataContext _context;

        public TipoAgendamentoController(DataContext context)
        {
            _context = context;
        }

        #region GET
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<TipoAgendamento>>> GetAllTipos()
        {
            try
            {
                var tipos = await _context.TiposAgendamento.ToListAsync();
                if (tipos == null || tipos.Count == 0)
                    return NotFound();

                return Ok(tipos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region GET {id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoAgendamento>> GetTipoPorId(int id)
        {
            try
            {
                var tipo = await _context.TiposAgendamento.FindAsync(id);
                if (tipo == null)
                    return NotFound();

                return Ok(tipo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region POST
        [HttpPost("AddTipo")]
        public async Task<ActionResult> PostTipo([FromBody] TipoCreateDTO dto)
        {
            try
            {
                if (await _context.TiposAgendamento.AnyAsync(tu => tu.Descricao.ToLower() == dto.Descricao.ToLower()))
                {
                    return BadRequest($"O tipo {dto.Descricao} informado já existe.");
                }

                var ultimoId = await _context.TiposAgendamento.OrderByDescending(x => x.IdTipoAgendamento).Select(x => x.IdTipoAgendamento).FirstOrDefaultAsync();

                var t = new TipoAgendamento();
                t.IdTipoAgendamento = ultimoId + 1;
                t.Descricao = dto.Descricao;

                _context.TiposAgendamento.Add(t);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTipoPorId), new { id = t.IdTipoAgendamento }, t);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar item: {ex.Message}");
            }
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public async Task<ActionResult> PutTipo(int id, [FromBody] TipoUpdateDTO dto)
        {
            try
            {
                if (!await _context.TiposAgendamento.AnyAsync(x => x.IdTipoAgendamento == id))
                    return NotFound($"O Tipo com o ID {id} não foi encontrado.");

                // Atualize os campos
                 var t = new TipoAgendamento();
                t.IdTipoAgendamento = dto.IdTipo;
                t.Descricao = dto.Descricao;


                _context.TiposAgendamento.Update(t);
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
        public async Task<ActionResult> DeleteTipo(int id)
        {
            try
            {
                var tipo = await _context.TiposAgendamento.FirstOrDefaultAsync(x => x.IdTipoAgendamento == id);
                if (tipo == null)
                    return NotFound($"O Tipo com o ID {id} não foi encontrado.");

                _context.TiposAgendamento.Remove(tipo);
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