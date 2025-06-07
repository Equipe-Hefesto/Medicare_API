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
    public class FormaPagamentoController : Controller
    {
        private readonly DataContext _context;

        public FormaPagamentoController(DataContext context)
        {
            _context = context;
        }

        #region GET
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<FormaPagamento>>> GetAllFormasPagamento()
        {
            try
            {
                var formas = await _context.FormasPagamento.ToListAsync();
                if (formas == null || formas.Count == 0)
                    return NotFound();

                return Ok(formas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region GET {id}
        [HttpGet("{id}")]
        
        public async Task<ActionResult<FormaPagamento>> GetFormaPagamentoPorId(int id)
        {
            try
            {
                var forma = await _context.FormasPagamento.FindAsync(id);
                if (forma == null)
                    return NotFound();

                return Ok(forma);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region POST
        [HttpPost]
        [Authorize(Roles ="ADMIN")]
        public async Task<ActionResult> PostFormaPagamento([FromBody] FormaPagamentoCreateDTO dto)
        {
            try
            {
                if (await _context.FormasPagamento.AnyAsync(fp => fp.Descricao.ToLower() == dto.Descricao.ToLower()))
                {
                    return BadRequest($"A forma pagamento {dto.Descricao} informada já existe.");
                }

                var ultimoId = await _context.FormasPagamento.OrderByDescending(x => x.IdFormaPagamento).Select(x => x.IdFormaPagamento).FirstOrDefaultAsync();

                var f = new FormaPagamento();
                f.IdFormaPagamento = ultimoId + 1;
                f.Descricao = dto.Descricao;
                f.QtdeMinParcelas = dto.QtdeMinParcelas;
                f.QtdeMaxParcelas = dto.QtdeMaxParcelas;
                f.QtdeParcelas = dto.QtdeParcelas;

                _context.FormasPagamento.Add(f);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetFormaPagamentoPorId), new { id = f.IdFormaPagamento }, f);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar item: {ex.Message}");
            }
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        
        public async Task<ActionResult> PutFormaPagamento(int id, [FromBody] FormaPagamentoUpdateDTO dto)
        {
            try
            {
                if (!await _context.FormasPagamento.AnyAsync(x => x.IdFormaPagamento == id))
                    return NotFound($"A forma pagamento com o ID {id} não foi encontrado.");

                // Atualize os campos
                var f = new FormaPagamento();
                f.IdFormaPagamento = dto.IdFormaPagamento;
                f.Descricao = dto.Descricao;
                f.QtdeMinParcelas = dto.QtdeMinParcelas;
                f.QtdeMaxParcelas = dto.QtdeMaxParcelas;
                f.QtdeParcelas = dto.QtdeParcelas;


                _context.FormasPagamento.Update(f);
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
        public async Task<ActionResult> DeleteFormaPagamento(int id)
        {
            try
            {
                var tipo = await _context.FormasPagamento.FirstOrDefaultAsync(x => x.IdFormaPagamento == id);
                if (tipo == null)
                    return NotFound($"A forma pagamento com o ID {id} não foi encontrada.");

                _context.FormasPagamento.Remove(tipo);
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