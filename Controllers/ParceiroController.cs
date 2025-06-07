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
    public class ParceiroController : Controller
    {
        private readonly DataContext _context;

        public ParceiroController(DataContext context)
        {
            _context = context;
        }

        #region GET
        [HttpGet]
   
        public async Task<ActionResult<IEnumerable<Parceiro>>> GetAllParceiros()
        {
            try
            {
                var entidades = await _context.Parceiros.ToListAsync();
                if (entidades == null || entidades.Count == 0)
                    return NotFound();

                return Ok(entidades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region GET {id}
        [HttpGet("{id}")]
       
        public async Task<ActionResult<Parceiro>> GetParceiro(int id)
        {
            try
            {
                var entidade = await _context.Parceiros.FindAsync(id);
                if (entidade == null)
                    return NotFound();

                return Ok(entidade);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region POST
        [HttpPost]
             public async Task<ActionResult> PostParceiro([FromBody] ParceiroCreateDTO dto)
        {
            try
            {
                if (await _context.Parceiros.AnyAsync(p => p.CNPJ == dto.CNPJ))
                {
                    return BadRequest($"O CNPJ {dto.CNPJ} informado já existe.");
                }


                var ultimoId = await _context.Parceiros.OrderByDescending(x => x.IdParceiro).Select(x => x.IdParceiro).FirstOrDefaultAsync();

                // Validações opcionais

                var p = new Parceiro();
                p.IdParceiro = ultimoId + 1;
                p.CNPJ = dto.CNPJ;
                p.Nome = dto.Nome;
                p.Apelido = dto.Apelido;
                p.Status = "A";

                _context.Parceiros.Add(p);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetParceiro), new { id = p.IdParceiro }, p);
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
        [Authorize(Roles = "PARCEIRO")]
        public async Task<ActionResult> PutParceiro(int id, [FromBody] ParceiroUpdateDTO dto)
        {
            try
            {
                if (await _context.Parceiros.AnyAsync(x => x.IdParceiro == id))
                    return NotFound($"O Parceiro com o ID {id} não foi encontrado.");


                // Atualize os campos
                var p = new Parceiro();
                p.CNPJ = dto.CNPJ;
                p.Nome = dto.Nome;
                p.Apelido = dto.Apelido;
                p.Status = "A";

                _context.Parceiros.Update(p);
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
        public async Task<ActionResult> DeleteParceiro(int id)
        {
            try
            {
                var parceiro = await _context.Parceiros.FirstOrDefaultAsync(x => x.IdParceiro == id);
                if (parceiro == null)
                    return NotFound($"O Parceiro com o ID {id} não foi encontrado.");

                _context.Parceiros.Remove(parceiro);
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
