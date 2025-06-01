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
    public class UtilizadorTipoUtilizadorController : Controller
    {
        private readonly DataContext _context;

        public UtilizadorTipoUtilizadorController(DataContext context)
        {
            _context = context;
        }

        #region GET
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
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
        [Authorize(Roles = "ADMIN")]
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
        [AllowAnonymous]
        public async Task<ActionResult> PostRelacionamento([FromBody] UtilizadorTipoUtilizadorCreateDTO dto)
        {
            try
            {
                var userString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userString, out int userId))
                {
                    return Unauthorized("Token inválido");
                }

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
                /*ut.IdUtilizador = dto.IdUtilizador;
                    Não iremos mais utilizar isso pois, o id do utlizador está no token
                */

                //Pega o Id do utilizador via Token
                ut.IdUtilizador = userId;
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
                // Pega o ID do usuário autenticado via token
                var userString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userString, out int userId))
                    return Unauthorized("Token inválido");

                // Confirma que o usuário só pode alterar os próprios dados
                if (idUtilizador != userId)
                    return Forbid("Você não pode alterar dados de outro usuário.");

                // Verifica se o relacionamento original existe
                var relacionamento = await _context.UtilizadoresTiposUtilizadores
                    .FirstOrDefaultAsync(ut => ut.IdUtilizador == idUtilizador && ut.IdTipoUtilizador == idTipo);

                if (relacionamento == null)
                    return NotFound("Relacionamento original não encontrado.");

                // Verifica se o novo relacionamento já existe (evita duplicação)
                bool relacionamentoDuplicado = await _context.UtilizadoresTiposUtilizadores
                    .AnyAsync(ut => ut.IdUtilizador == userId && ut.IdTipoUtilizador == dto.IdTipoUtilizador);

                if (relacionamentoDuplicado)
                    return BadRequest("O novo relacionamento já existe.");

                // Busca o novo tipo de utilizador
                var tipo = await _context.TiposUtilizadores
                    .FirstOrDefaultAsync(t => t.IdTipoUtilizador == dto.IdTipoUtilizador);

                if (tipo == null)
                    return BadRequest($"O Tipo com ID {dto.IdTipoUtilizador} não foi encontrado.");

                // Atualiza o relacionamento existente
                relacionamento.IdTipoUtilizador = dto.IdTipoUtilizador;
                relacionamento.TipoUtilizador = tipo;

                _context.UtilizadoresTiposUtilizadores.Update(relacionamento);
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

        //Revisar isso pois não está funcionando        
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