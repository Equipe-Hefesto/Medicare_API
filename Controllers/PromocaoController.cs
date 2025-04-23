using Medicare_API.Data;
using Medicare_API.Models;
using Medicare_API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medicare_API.Controllers
{
    [Route("[controller]")]
    public class PromocaoController : Controller
    {
        private readonly DataContext _context;

        public PromocaoController(DataContext context)
        {
            _context = context;
        }

        #region GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Promocao>>> GetAllPromocoes()
        {
            try
            {
                var promocoes = await _context.Promocoes.ToListAsync();
                if (promocoes == null || promocoes.Count == 0)
                    return NotFound();

                return Ok(promocoes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region GET {id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Promocao>> GetPromocaoPorId(int id)
        {
            try
            {
                var promocao = await _context.Promocoes.FindAsync(id);
                if (promocao == null)
                    return NotFound();

                return Ok(promocao);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region POST
        [HttpPost]
        public async Task<ActionResult> PostPromocao([FromBody] PromocaoCreateDTO dto)
        {
            try
            {
                if (await _context.Promocoes.AnyAsync(tu => tu.Descricao.ToLower() == dto.Descricao.ToLower()))
                {
                    return BadRequest($"A promoção {dto.Descricao} informada já existe.");
                }

                var remedio = await _context.Remedios.FirstOrDefaultAsync(r => r.IdRemedio == dto.IdRemedio);
                var formaPagamento = await _context.FormasPagamento.FirstOrDefaultAsync(fp => fp.IdFormaPagamento == dto.IdFormaPagamento);;
                var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.IdUtilizador == dto.IdUtilizador);

                var ultimoId = await _context.Promocoes.OrderByDescending(x => x.IdPromocao).Select(x => x.IdPromocao).FirstOrDefaultAsync();

                var p = new Promocao();
                p.IdPromocao = ultimoId + 1;
                p.IdRemedio = dto.IdRemedio;
                p.IdFormaPagamento = dto.IdFormaPagamento;
                p.IdUtilizador = dto.IdUtilizador;
                p.Descricao = dto.Descricao;
                p.DataInicio = dto.DataInicio;
                p.DataFim = dto.DataFim;
                p.Valor = dto.Valor;
                p.Remedio = remedio;
                p.FormaPagamento = formaPagamento;
                p.Utilizador = utilizador;

                _context.Promocoes.Add(p);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPromocaoPorId), new { id = p.IdPromocao }, p);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar item: {ex.Message}");
            }
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public async Task<ActionResult> PutPromocao(int id, [FromBody] PromocaoUpdateDTO dto)
        {
            try
            {
                if (!await _context.Promocoes.AnyAsync(x => x.IdPromocao == id))
                    return NotFound($"A Promocao com o ID {id} não foi encontrada.");

                // Atualize os campos
                var remedio = await _context.Remedios.FirstOrDefaultAsync(r => r.IdRemedio == dto.IdRemedio);
                var formaPagamento = await _context.FormasPagamento.FirstOrDefaultAsync(fp => fp.IdFormaPagamento == dto.IdFormaPagamento);;
                var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.IdUtilizador == dto.IdUtilizador);

                var p = new Promocao();
                p.IdPromocao = dto.IdPromocao;
                p.IdRemedio = dto.IdRemedio;
                p.IdFormaPagamento = dto.IdFormaPagamento;
                p.IdUtilizador = dto.IdUtilizador;
                p.Descricao = dto.Descricao;
                p.DataInicio = dto.DataInicio;
                p.DataFim = dto.DataFim;
                p.Valor = dto.Valor;
                p.Remedio = remedio;
                p.FormaPagamento = formaPagamento;
                p.Utilizador = utilizador;


                _context.Promocoes.Update(p);
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
        public async Task<ActionResult> DeletePromocao(int id)
        {
            try
            {
                var promocao = await _context.Promocoes.FirstOrDefaultAsync(x => x.IdPromocao == id);
                if (promocao == null)
                    return NotFound($"A Promocao com o ID {id} não foi encontrada.");

                _context.Promocoes.Remove(promocao);
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