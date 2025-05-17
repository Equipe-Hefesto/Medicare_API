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
    public class TipoUtilizadorController : Controller
    {
        private readonly DataContext _context;

        public TipoUtilizadorController(DataContext context)
        {
            _context = context;
        }

        #region GET
        [Authorize(Roles ="ADMIN")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoUtilizador>>> GetAllTipos()
        {
            try
            {
                var tipos = await _context.TiposUtilizadores.ToListAsync();
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
        [Authorize(Roles ="ADMIN")]
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoUtilizador>> GetTipoPorId(int id)
        {
            try
            {
                var tipo = await _context.TiposUtilizadores.FindAsync(id);
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
        [Authorize(Roles ="ADMIN")]
        [HttpPost]
        public async Task<ActionResult> PostTipo([FromBody] TipoCreateDTO dto)
        {
            try
            {
                if (await _context.TiposUtilizadores.AnyAsync(tu => tu.Descricao.ToLower() == dto.Descricao.ToLower()))
                {
                    return BadRequest($"O tipo {dto.Descricao} informado já existe.");
                }

                var ultimoId = await _context.TiposUtilizadores.OrderByDescending(x => x.IdTipoUtilizador).Select(x => x.IdTipoUtilizador).FirstOrDefaultAsync();

                var t = new TipoUtilizador();
                t.IdTipoUtilizador = ultimoId + 1;
                t.Descricao = dto.Descricao;

                _context.TiposUtilizadores.Add(t);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTipoPorId), new { id = t.IdTipoUtilizador }, t);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar item: {ex.Message}");
            }
        }
        #endregion

        #region PUT
        [Authorize(Roles ="ADMIN")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutTipo(int id, [FromBody] TipoUpdateDTO dto)
        {
            try
            {
                if (!await _context.TiposUtilizadores.AnyAsync(x => x.IdTipoUtilizador == id))
                    return NotFound($"O Tipo com o ID {id} não foi encontrado.");

                // Atualize os campos
                 var t = new TipoUtilizador();
                t.IdTipoUtilizador = dto.IdTipo;
                t.Descricao = dto.Descricao;


                _context.TiposUtilizadores.Update(t);
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
        [Authorize(Roles ="ADMIN")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTipo(int id)
        {
            try
            {
                var tipo = await _context.TiposUtilizadores.FirstOrDefaultAsync(x => x.IdTipoUtilizador == id);
                if (tipo == null)
                    return NotFound($"O Tipo com o ID {id} não foi encontrado.");

                _context.TiposUtilizadores.Remove(tipo);
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
