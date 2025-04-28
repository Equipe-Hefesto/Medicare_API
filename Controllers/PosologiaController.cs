using Medicare_API.Data;
using Medicare_API.Models;
using Medicare_API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medicare_API.Controllers
{
    [Route("[controller]")]
    public class PosologiaController : Controller
    {
        private readonly DataContext _context;

        public PosologiaController(DataContext context)
        {
            _context = context;
        }

        #region GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Posologia>>> GetAllPosologias()
        {
            try
            {
                var posologias = await _context.Posologias.ToListAsync();

                if (posologias == null || posologias.Count == 0)
                    return NotFound();

                return Ok(posologias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region GET {id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Posologia>> GetPosologiaPorId(int id)
        {
            try
            {
                var posologia = await _context.Posologias.FirstOrDefaultAsync(p => p.IdPosologia == id);

                if (posologia == null)
                    return NotFound();

                return Ok(posologia);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region POST
        [HttpPost]
        public async Task<ActionResult> PostPosologia([FromBody] PosologiaCreateDTO dto)
        {
            try
            {
                if (await _context.Posologias
                    .AnyAsync(p => p.IdRemedio == dto.IdRemedio && p.IdUtilizador == dto.IdUtilizador && p.Quantidade == dto.Quantidade && p.IdTipoFarmaceutico == dto.IdTipoFarmaceutico))
                {
                    return BadRequest($"Já existe uma posologia para o Remédio {dto.IdRemedio} e Utilizador {dto.IdUtilizador} iniciada na data {dto.DataInicio}.");
                }

                var remedio = await _context.Remedios.FirstOrDefaultAsync(r => r.IdRemedio == dto.IdRemedio);
                var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.IdUtilizador == dto.IdUtilizador);
                var tipoFarmaceutico = await _context.TiposFarmaceutico.FirstOrDefaultAsync(f => f.IdTipoFarmaceutico == dto.IdTipoFarmaceutico);
                var tipoGrandeza = await _context.TiposGrandeza.FirstOrDefaultAsync(g => g.IdTipoGrandeza == dto.IdTipoGrandeza);
                var tipoAgendamento = await _context.TiposAgendamento.FirstOrDefaultAsync(a => a.IdTipoAgendamento == dto.IdTipoAgendamento);

                var ultimoId = await _context.Posologias.OrderByDescending(p => p.IdPosologia).Select(p => p.IdPosologia).FirstOrDefaultAsync();

                var p = new Posologia();

                p.IdPosologia = ultimoId + 1;
                p.IdRemedio = dto.IdRemedio;
                p.IdUtilizador = dto.IdUtilizador;
                p.IdTipoFarmaceutico = dto.IdTipoFarmaceutico;
                p.IdTipoGrandeza = dto.IdTipoGrandeza;
                p.IdTipoAgendamento = dto.IdTipoAgendamento;
                p.Quantidade = dto.Quantidade;
                p.QuantidadeDose = dto.QuantidadeDose;
                p.DataInicio = dto.DataInicio;
                p.DataFim = dto.DataFim;
                p.Intervalo = dto.Intervalo!;
                p.DiasSemana = dto.DiasSemana!;
                p.DiasUso = dto.DiasUso;
                p.DiasPausa = dto.DiasPausa;
                p.Remedio = remedio!;
                p.Utilizador = utilizador!;
                p.TipoFarmaceutico = tipoFarmaceutico!;
                p.TipoGrandeza = tipoGrandeza!;
                p.TipoAgendamento = tipoAgendamento!;


                _context.Posologias.Add(p);
                await _context.SaveChangesAsync();



                await _context.SaveChangesAsync();


                return CreatedAtAction(nameof(GetPosologiaPorId), new { id = p.IdPosologia }, p);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar item: {ex.Message}");
            }
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public async Task<ActionResult> PutPosologia(int id, [FromBody] PosologiaUpdateDTO dto)
        {
            try
            {

                if (!await _context.Posologias
                    .AnyAsync(p => p.IdRemedio == dto.IdRemedio && p.IdUtilizador == dto.IdUtilizador && p.DataInicio == dto.DataInicio))
                {
                    return BadRequest($"Não existe uma posologia para o Remédio {dto.IdRemedio} e Utilizador {dto.IdUtilizador} iniciada na data {dto.DataInicio}.");
                }

                var remedio = await _context.Remedios.FirstOrDefaultAsync(r => r.IdRemedio == dto.IdRemedio);
                var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.IdUtilizador == dto.IdUtilizador);
                var tipoFarmaceutico = await _context.TiposFarmaceutico.FirstOrDefaultAsync(f => f.IdTipoFarmaceutico == dto.IdTipoFarmaceutico);
                var tipoGrandeza = await _context.TiposGrandeza.FirstOrDefaultAsync(g => g.IdTipoGrandeza == dto.IdTipoGrandeza);
                var tipoAgendamento = await _context.TiposAgendamento.FirstOrDefaultAsync(a => a.IdTipoAgendamento == dto.IdTipoAgendamento);


                var p = new Posologia();

                p.IdPosologia = dto.IdPosologia;
                p.IdRemedio = dto.IdRemedio;
                p.IdUtilizador = dto.IdUtilizador;
                p.IdTipoFarmaceutico = dto.IdTipoFarmaceutico;
                p.IdTipoGrandeza = dto.IdTipoGrandeza;
                p.IdTipoAgendamento = dto.IdTipoAgendamento;
                p.Quantidade = dto.Quantidade;
                p.QuantidadeDose = dto.QuantidadeDose;
                p.DataInicio = dto.DataInicio;
                p.DataFim = dto.DataFim;
                p.Intervalo = dto.Intervalo!;
                p.DiasSemana = dto.DiasSemana!;
                p.DiasUso = dto.DiasUso;
                p.DiasPausa = dto.DiasPausa;
                p.Remedio = remedio!;
                p.Utilizador = utilizador!;
                p.TipoFarmaceutico = tipoFarmaceutico!;
                p.TipoGrandeza = tipoGrandeza!;
                p.TipoAgendamento = tipoAgendamento!;

                _context.Posologias.Update(p);
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
        public async Task<ActionResult> DeletePosologia(int id)
        {
            try
            {
                var posologia = await _context.Posologias.FindAsync(id);
                if (posologia == null)
                    return NotFound($"A Posologia com o ID {id} não foi encontrada.");

                _context.Posologias.Remove(posologia);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar item: {ex.Message}");
            }
        }
        #endregion
        #region Adicionais

        // Método privado para obter o nome do remédio
        private string GetNomeRemedio(int idRemedio)
        {
            return _context.Remedios.Find(idRemedio)?.Nome ?? "Medicamento";
        }
         private string GetTipoFarmaceutico(int IdTipoFarmaceutico)
        {
            return _context.TiposFarmaceutico.Find(IdTipoFarmaceutico)?.Descricao ?? "Tipo Farmaceutico";
        }
          private string GetTipoGrandeza(int IdTipoGrandeza)
        {
            return _context.TiposGrandeza.Find(IdTipoGrandeza)?.Descricao ?? "Tipo Gradeza";
        }
        
        
        #endregion
    }
}