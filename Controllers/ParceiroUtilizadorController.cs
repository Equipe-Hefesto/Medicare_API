using Medicare_API.Data;
using Medicare_API.Models;
using Medicare_API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medicare_API.Controllers
{
    [Route("[controller]")]
    public class ParceiroUtilizadorController : Controller
    {
        private readonly DataContext _context;

        public ParceiroUtilizadorController(DataContext context)
        {
            _context = context;
        }

        #region GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parceiro>>> GetAllRelacionamentos()
        {
            try
            {
                var relacionamentos = await _context.ParceirosUtilizadores.ToListAsync();
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
        [HttpGet("{idParceiro}/{idUtilizador}")]
        public async Task<ActionResult<Parceiro>> GetRelacionamentoPorIds(int idParceiro, int idUtilizador)
        {
            try
            {
                var relacionamento = await _context.ParceirosUtilizadores.FirstOrDefaultAsync(pc => pc.IdParceiro == idParceiro && pc.IdUtilizador == idUtilizador);
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
        public async Task<ActionResult> PostRelacionamento([FromBody] ParceiroUtilizadorCreateDTO dto)
        {
            try
            {
                var parceiro = await _context.Parceiros.FirstOrDefaultAsync(u => u.IdParceiro == dto.IdParceiro);
                if (parceiro == null)
                    return BadRequest($"O Parceiro com o ID {dto.IdParceiro} não existe.");


                // Validar se o Tipo existe
                var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(t => t.IdUtilizador == dto.IdUtilizador);
                if (utilizador == null)
                    return BadRequest($"O Utilizador com o ID {dto.IdUtilizador} não existe.");

                // Validar se o relacionamento já existe
                if (await _context.ParceirosUtilizadores.AnyAsync(pu => pu.IdParceiro == dto.IdParceiro && pu.IdUtilizador == dto.IdUtilizador))
                {
                    return BadRequest($"A relação IdParceiro {dto.IdParceiro} + IdUtilizador {dto.IdUtilizador} já existe.");
                }

                var pc = new ParceiroUtilizador();
                pc.IdParceiro = dto.IdParceiro;
                pc.IdUtilizador = dto.IdUtilizador;
                pc.Parceiro = parceiro;
                pc.Utilizador = utilizador;

                _context.ParceirosUtilizadores.Add(pc);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRelacionamentoPorIds), new { idParceiro = pc.IdParceiro, idUtilizador = pc.IdUtilizador }, pc);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar item: {ex.Message}");
            }
        }
        #endregion

        #region PUT
        [HttpPut("{idParceiro}/{idUtilizador}")]
        public async Task<ActionResult> PutRelacionamento(int idParceiro, int idUtilizador, [FromBody] ParceiroUtilizadorUpdateDTO dto)
        {
            try
            {
                var parceiro = await _context.Parceiros.FirstOrDefaultAsync(u => u.IdParceiro == dto.IdParceiro);
                if (parceiro == null)
                    return BadRequest($"O Parceiro com o ID {dto.IdParceiro} não existe.");


                // Validar se o Tipo existe
                var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(t => t.IdUtilizador == dto.IdUtilizador);
                if (utilizador == null)
                    return BadRequest($"O Utilizador com o ID {dto.IdUtilizador} não existe.");

                // Validar se o relacionamento já existe
                var pc = await _context.ParceirosUtilizadores.FirstOrDefaultAsync(pu => pu.IdParceiro == dto.IdParceiro && pu.IdUtilizador == dto.IdUtilizador);
                if (pc == null)
                {
                    return BadRequest($"A relação IdParceiro {dto.IdParceiro} + IdUtilizador {dto.IdUtilizador} não existe.");
                }
            
                pc.IdParceiro = dto.IdParceiro;
                pc.IdUtilizador = dto.IdUtilizador;
                pc.Parceiro = parceiro;
                pc.Utilizador = utilizador;

                _context.ParceirosUtilizadores.Update(pc);
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
        [HttpDelete("{idParceiro}/{idUtilizador}")]
        public async Task<ActionResult> DeleteParceiro(int idParceiro, int idUtilizador)
        {
            try
            {
                 var relacionamento = await _context.ParceirosUtilizadores.FirstOrDefaultAsync(pu => pu.IdParceiro == idParceiro && pu.IdUtilizador == idUtilizador);
                if (relacionamento == null)
                {
                    return BadRequest($"A relação IdParceiro {idParceiro} + IdUtilizador {idUtilizador} não existe.");
                }
                _context.ParceirosUtilizadores.Remove(relacionamento);
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
