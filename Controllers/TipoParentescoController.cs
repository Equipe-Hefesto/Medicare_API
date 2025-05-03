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
    public class TipoParentescoController : Controller
    {
        private readonly DataContext _context;

        public TipoParentescoController(DataContext context)
        {
            _context = context;
        }

        #region GET
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<TipoParentesco>>> GetAllTipos()
        {
            try
            {
                var tipos = await _context.TiposParentesco.ToListAsync();
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
        public async Task<ActionResult<TipoParentesco>> GetTipoPorId(int id)
        {
            try
            {
                var tipo = await _context.TiposParentesco.FindAsync(id);
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
                if (await _context.TiposParentesco.AnyAsync(tu => tu.Descricao.ToLower() == dto.Descricao.ToLower()))
                {
                    return BadRequest($"O tipo {dto.Descricao} informado já existe.");
                }

                var ultimoId = await _context.TiposParentesco.OrderByDescending(x => x.IdTipoParentesco).Select(x => x.IdTipoParentesco).FirstOrDefaultAsync();

                var t = new TipoParentesco();
                t.IdTipoParentesco = ultimoId + 1;
                t.Descricao = dto.Descricao;

                _context.TiposParentesco.Add(t);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTipoPorId), new { id = t.IdTipoParentesco }, t);
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
                if (!await _context.TiposParentesco.AnyAsync(x => x.IdTipoParentesco == id))
                    return NotFound($"O Tipo com o ID {id} não foi encontrado.");

                // Atualize os campos
                 var t = new TipoParentesco();
                t.IdTipoParentesco = dto.IdTipo;
                t.Descricao = dto.Descricao;


                _context.TiposParentesco.Update(t);
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
                var tipo = await _context.TiposParentesco.FirstOrDefaultAsync(x => x.IdTipoParentesco == id);
                if (tipo == null)
                    return NotFound($"O Tipo com o ID {id} não foi encontrado.");

                _context.TiposParentesco.Remove(tipo);
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