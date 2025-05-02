using Medicare_API.Data;
using Medicare_API.Models;
using Medicare_API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medicare_API.Controllers
{
    [Route("[controller]")]
    public class UtilizadorTipoUtilizadorController : Controller
    {
        private readonly DataContext _context;

        public UtilizadorTipoUtilizadorController(DataContext context)
        {
            _context = context;
        }

        #region GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UtilizadorTipoUtilizador>>> GetAllRelacionamentos()
        {
            try
            {
                var relacionamentos = await _context.UtilizadoresTiposUtilizadores.ToListAsync();
                if (relacionamentos == null || relacionamentos.Count == 0)
                {
                    return NotFound();
                }

                return Ok(relacionamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region GET {id}
        [HttpGet("{idUtilizador}/{idTipo}")]
        public async Task<ActionResult<UtilizadorTipoUtilizador>> GetRelacionamentoPorIds(int idUtilizador, int idTipo)
        {
            try
            {
                var relacionamento = await _context.UtilizadoresTiposUtilizadores.FirstOrDefaultAsync(ut => ut.IdUtilizador == idUtilizador && ut.IdTipoUtilizador == idTipo);
                if (relacionamento == null)
                {
                    return NotFound();
                }

                return Ok(relacionamento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region POST
        [HttpPost]
        public async Task<ActionResult> PostRelacionamento([FromBody] UtilizadorTipoUtilizadorCreateDTO dto)
        {
            try
            {
                // Validar se o Utilizador existe
                var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.IdUtilizador == dto.IdUtilizador);
                if (utilizador == null)
                    return BadRequest($"O Utilizador com o ID {dto.IdUtilizador} não existe.");

                // Validar se o Tipo existe
                var tipo = await _context.TiposUtilizadores.FirstOrDefaultAsync(t => t.IdTipoUtilizador == dto.IdTipoUtilizador);
                if (tipo == null)
                    return BadRequest($"O Tipo com o ID {dto.IdTipoUtilizador} não existe.");

                // Validar se o relacionamento já existe
                if (await _context.UtilizadoresTiposUtilizadores.AnyAsync(ut => ut.IdUtilizador == dto.IdUtilizador && ut.IdTipoUtilizador == dto.IdTipoUtilizador))
                {
                    return BadRequest($"A relação IdUtilizador {dto.IdUtilizador} + IdTipoUtilizador {dto.IdTipoUtilizador} já existe.");
                }


                //Validar informações


                var ut = new UtilizadorTipoUtilizador();
                ut.IdUtilizador = dto.IdUtilizador;
                ut.IdTipoUtilizador = dto.IdTipoUtilizador;
                ut.Utilizador = utilizador;
                ut.TipoUtilizador = tipo;


                _context.UtilizadoresTiposUtilizadores.Add(ut);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRelacionamentoPorIds), new { idUtilizador = ut.IdUtilizador, idTipo = ut.IdTipoUtilizador }, ut);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar item: {ex.Message}");
            }
        }
        #endregion

        #region PUT
        [HttpPut("{idUtilizador}/{idTipo}")]
        public async Task<ActionResult> PutRelacionamento(int idUtilizador, int idTipo, [FromBody] UtilizadorTipoUtilizadorUpdateDTO dto)
        {
            try
            {
                var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.IdUtilizador == dto.IdUtilizador);
                if (utilizador == null)
                    return BadRequest($"O Utilizador com o ID {dto.IdUtilizador} não foi encontrado.");

                // Validar se o Tipo existe
                var tipo = await _context.TiposUtilizadores.FirstOrDefaultAsync(t => t.IdTipoUtilizador == dto.IdTipoUtilizador);
                if (tipo == null)
                    return BadRequest($"O Tipo com o ID {dto.IdTipoUtilizador} não foi encontrado.");

                // Validar se o relacionamento já existe
                if (!await _context.UtilizadoresTiposUtilizadores.AnyAsync(ut => ut.IdUtilizador == dto.IdUtilizador && ut.IdTipoUtilizador == dto.IdTipoUtilizador))
                {
                    return BadRequest($"A relação IdUtilizador {dto.IdUtilizador} + IdTipoUtilizador {dto.IdTipoUtilizador} não foi encontrado.");
                }

                //Validar informações

                var ut = new UtilizadorTipoUtilizador();
                ut.IdUtilizador = dto.IdUtilizador;
                ut.IdTipoUtilizador = dto.IdTipoUtilizador;
                ut.Utilizador = utilizador;
                ut.TipoUtilizador = tipo;


                _context.UtilizadoresTiposUtilizadores.Update(ut);
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
        [HttpDelete("{idUtilizador}/{idTipo}")]
        public async Task<ActionResult> DeleteCuidador(int idUtilizador, int idTipo)
        {
            try
            {
                // Validar se o relacionamento já existe
                var relacionamento = await _context.UtilizadoresTiposUtilizadores.FirstOrDefaultAsync(ut => ut.IdUtilizador == idUtilizador && ut.IdTipoUtilizador == idTipo);
                if (relacionamento == null)
                {
                    return BadRequest($"A relação IdUtilizador {idUtilizador} + IdTipoUtilizador {idTipo} não foi encontrado.");
                }
                _context.UtilizadoresTiposUtilizadores.Remove(relacionamento);
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