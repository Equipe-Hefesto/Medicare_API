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
                    .AnyAsync(p => p.IdRemedio == dto.IdRemedio && p.IdUtilizador == dto.IdUtilizador && p.DataInicio == dto.DataInicio))
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

                DateTime dataAtual = p.DataInicio;
                DateTime dataFim = p.DataFim != default ? p.DataFim : DateTime.Now.AddMonths(1);


                if (p.IdTipoAgendamento == 2 && dto.Horarios == null)
                {
                    TimeSpan intervaloHoras = TimeSpan.Zero;
                    intervaloHoras = TimeSpan.FromHours(int.Parse(p.Intervalo));

                    while (dataAtual <= dataFim)
                    {
                        var h = new Horario();
                        h.Hora = TimeOnly.FromDateTime(dataAtual);
                        h.IdPosologia = p.IdPosologia;
                        h.Posologia = p;

                        _context.Horarios.Add(h);
                        await _context.SaveChangesAsync();
                        dataAtual = dataAtual.Add(intervaloHoras);

                    }
                }
                else
                {
                    // Associa os horários à posologia
                    foreach (var horario in dto.Horarios!)
                    {
                        var h = new Horario();
                        h.Hora = horario;
                        h.IdPosologia = p.IdPosologia;
                        h.Posologia = p;

                        _context.Horarios.Add(h);
                        await _context.SaveChangesAsync();
                    }
                }
  

                await _context.SaveChangesAsync();

                // Cria os alarmes para a posologia
                var alarmes = GerarAlarmes(p, await _context.Horarios.Where(h => h.IdPosologia == p.IdPosologia).ToListAsync());
                foreach (var alarme in alarmes)
                {
                    _context.Alarmes.Add(alarme);
                }

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
        #region 

        // Método privado para gerar alarmes
        private List<Alarme> GerarAlarmes(Posologia posologia, List<Horario> horarios)
        {
            var alarmes = new List<Alarme>();
            DateTime dataAtual = posologia.DataInicio;
            DateTime dataFim = posologia.DataFim != default ? posologia.DataFim : DateTime.Now.AddMonths(1);

            TimeSpan intervaloHoras = TimeSpan.Zero;
            if (posologia.IdTipoAgendamento == 1)  // Se for agendamento a cada X horas
                intervaloHoras = TimeSpan.FromHours(int.Parse(posologia.Intervalo));

            while (dataAtual <= dataFim)
            {
                if (ValidarDia(posologia, dataAtual))
                {
                    foreach (var horario in horarios)
                    {
                        var dataHora = dataAtual.Date.Add(horario.Hora.ToTimeSpan());
                        if (dataHora >= posologia.DataInicio && dataHora <= dataFim)
                        {
                            alarmes.Add(new Alarme
                            {
                                IdPosologia = posologia.IdPosologia,
                                DataHora = dataHora,
                                Descricao = $"Tomar {posologia.QuantidadeDose} de {GetNomeRemedio(posologia.IdRemedio)}",
                                Status = "pendente"
                            });
                        }
                    }
                }

                dataAtual = posologia.IdTipoAgendamento == 1 ?
                    dataAtual.Add(intervaloHoras) :
                    dataAtual.AddDays(1);
            }

            return alarmes;
        }

        // Método privado para validar o dia
        private bool ValidarDia(Posologia posologia, DateTime dia)
        {
            if (posologia.IdTipoAgendamento == 3)  // Se o agendamento for alternado
            {
                int ciclo = posologia.DiasUso + posologia.DiasPausa;
                int diasDesdeInicio = (dia - posologia.DataInicio).Days;
                return diasDesdeInicio % ciclo < posologia.DiasUso;
            }

            if (!string.IsNullOrWhiteSpace(posologia.DiasSemana))
            {
                var diasSemana = posologia.DiasSemana.Split(',').Select(int.Parse).ToList();
                return diasSemana.Contains((int)dia.DayOfWeek);
            }

            return true;
        }

        // Método privado para obter o nome do remédio
        private string GetNomeRemedio(int idRemedio)
        {
            return _context.Remedios.Find(idRemedio)?.Nome ?? "Medicamento";
        }
        #endregion
    }
}