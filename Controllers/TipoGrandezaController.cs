using Medicare_API.Data;
using Medicare_API.Models;
using Medicare_API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medicare_API.Controllers
{   
    [Route("[controller]")]
    public class TipoGrandezaController : Controller
    {
        private readonly DataContext _context;

        public TipoGrandezaController(DataContext context)
        {
            _context = context;
        }

        #region GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoGrandeza>>> GetAllTipos()
        {
            try
            {
                var tipos = await _context.TiposGrandeza.ToListAsync();
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
        public async Task<ActionResult<TipoGrandeza>> GetTipoPorId(int id)
        {
            try
            {
                var tipo = await _context.TiposGrandeza.FindAsync(id);
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
        [HttpPost]
        public async Task<ActionResult> PostTipo([FromBody] TipoCreateDTO dto)
        {
            try
            {
                if (await _context.TiposGrandeza.AnyAsync(tu => tu.Descricao.ToLower() == dto.Descricao.ToLower()))
                {
                    return BadRequest($"O tipo {dto.Descricao} informado já existe.");
                }

                var ultimoId = await _context.TiposGrandeza.OrderByDescending(x => x.IdTipoGrandeza).Select(x => x.IdTipoGrandeza).FirstOrDefaultAsync();

                var t = new TipoGrandeza();
                t.IdTipoGrandeza = ultimoId + 1;
                t.Descricao = dto.Descricao;

                _context.TiposGrandeza.Add(t);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTipoPorId), new { id = t.IdTipoGrandeza }, t);
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
                if (!await _context.TiposGrandeza.AnyAsync(x => x.IdTipoGrandeza == id))
                    return NotFound($"O Tipo com o ID {id} não foi encontrado.");

                // Atualize os campos
                var t = new TipoGrandeza();
                t.IdTipoGrandeza = dto.IdTipo;
                t.Descricao = dto.Descricao;


                _context.TiposGrandeza.Update(t);
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
                var tipo = await _context.TiposGrandeza.FirstOrDefaultAsync(x => x.IdTipoGrandeza == id);
                if (tipo == null)
                    return NotFound($"O Tipo com o ID {id} não foi encontrado.");

                _context.TiposGrandeza.Remove(tipo);
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