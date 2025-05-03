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
    public class RemedioController : Controller
    {
        private readonly DataContext _context;

        public RemedioController(DataContext context)
        {
            _context = context;
        }

        #region GET
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Remedio>>> GetAllRemedios()
        {
            try
            {
                var remedios = await _context.Remedios.ToListAsync();
                if (remedios == null || remedios.Count == 0)
                    return NotFound();

                return Ok(remedios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region GET {id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Remedio>> GetRemedioPorId(int id)
        {
            try
            {
                var remedio = await _context.Remedios.FindAsync(id);
                if (remedio == null)
                    return NotFound();

                return Ok(remedio);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region POST
        [HttpPost("AddRemedio")]
        public async Task<ActionResult> PostRemedio([FromBody] RemedioCreateDTO dto)
        {
            try
            {
                if (await _context.Remedios.AnyAsync(r => r.Nome.ToLower() == dto.Nome.ToLower()))
                {
                    return BadRequest($"O remedio {dto.Nome} informado já existe.");
                }

                var ultimoId = await _context.Remedios.OrderByDescending(x => x.IdRemedio).Select(x => x.IdRemedio).FirstOrDefaultAsync();

                var r = new Remedio();
                r.IdRemedio = ultimoId + 1;
                r.Nome = dto.Nome;
                r.DataCriacao = DateTime.Now;
                r.DataAtualizacao = DateTime.Now;

                _context.Remedios.Add(r);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRemedioPorId), new { id = r.IdRemedio }, r);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar item: {ex.Message}");
            }
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public async Task<ActionResult> PutTipo(int id, [FromBody] RemedioUpdateDTO dto)
        {
            try
            {
                if (!await _context.Remedios.AnyAsync(x => x.IdRemedio == id))
                    return NotFound($"O Remedio com o ID {id} não foi encontrado.");

                // Atualize os campos
                 var r = new Remedio();
                r.IdRemedio = dto.IdRemedio;
                r.Nome = dto.Nome;
                r.DataAtualizacao = DateTime.Now;

                _context.Remedios.Update(r);
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
                var remedio = await _context.Remedios.FirstOrDefaultAsync(x => x.IdRemedio == id);
                if (remedio == null)
                    return NotFound($"O Remedio com o ID {id} não foi encontrado.");

                _context.Remedios.Remove(remedio);
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
