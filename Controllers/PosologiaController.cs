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
        [HttpGet("GetAll")]
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
        [HttpPost("AddPoso")]
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


                await PostHorarios(p, dto.Horarios);

                switch (p.IdTipoAgendamento)
                {
                    case 1:
                        await AgendamentoPorHora(p);
                        break;
                    case 2:
                        await AgendamentoPorHora(p);
                        break;
                    case 3:
                        await AgendamentoPorDiaSemana(p);
                        break;
                    case 4:
                        await AgendamentoPorCiclo(p);
                        break;
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

                var p = await _context.Posologias.FirstOrDefaultAsync(p => p.IdPosologia == id);
                if (p == null)
                    return NotFound($"Posologia com ID {id} não encontrada.");

                var remedio = await _context.Remedios.FirstOrDefaultAsync(r => r.IdRemedio == dto.IdRemedio);
                var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.IdUtilizador == dto.IdUtilizador);
                var tipoFarmaceutico = await _context.TiposFarmaceutico.FirstOrDefaultAsync(f => f.IdTipoFarmaceutico == dto.IdTipoFarmaceutico);
                var tipoGrandeza = await _context.TiposGrandeza.FirstOrDefaultAsync(g => g.IdTipoGrandeza == dto.IdTipoGrandeza);
                var tipoAgendamento = await _context.TiposAgendamento.FirstOrDefaultAsync(a => a.IdTipoAgendamento == dto.IdTipoAgendamento);

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
                p.DiasSemana = dto.DiasSemana;
                p.DiasUso = dto.DiasUso;
                p.DiasPausa = dto.DiasPausa;
                p.Remedio = remedio!;
                p.Utilizador = utilizador!;
                p.TipoFarmaceutico = tipoFarmaceutico!;
                p.TipoGrandeza = tipoGrandeza!;
                p.TipoAgendamento = tipoAgendamento!;

                _context.Posologias.Update(p);
                await _context.SaveChangesAsync();

                var horarios = _context.Horarios.Where(h => h.IdPosologia == p.IdPosologia).ToList();
                var alarmes = _context.Alarmes.Where(a => a.IdPosologia == p.IdPosologia).ToList();

                _context.Horarios.RemoveRange(horarios);
                _context.Alarmes.RemoveRange(alarmes);
                await _context.SaveChangesAsync();

                await PostHorarios(p, dto.Horarios);

                switch (p.IdTipoAgendamento)
                {
                    case 1:
                        await AgendamentoPorHora(p);
                        break;
                    case 2:
                        await AgendamentoPorHora(p);
                        break;
                    case 3:
                        await AgendamentoPorDiaSemana(p);
                        break;
                    case 4:
                        await AgendamentoPorCiclo(p);
                        break;
                }

                return Ok();
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


        private async Task PostHorarios(Posologia p, List<string> lista)
        {
            List<string> horarios = new List<string>();

            if (lista == null || p.IdTipoAgendamento == 2)
            {
                DateTime dtAtual = p.DataInicio;
                var intervaloHoras = TimeSpan.FromHours(int.Parse(p.Intervalo));

                while (dtAtual <= p.DataInicio.AddHours(24))
                {
                    horarios.Add(TimeOnly.FromDateTime(dtAtual).ToString());
                    dtAtual = dtAtual.Add(intervaloHoras);
                }
            }
            else horarios = lista;

            // Associa os horários à posologia
            foreach (var horario in horarios)
            {
                var hora = TimeOnly.Parse(horario);

                bool jaExiste = await _context.Horarios.AnyAsync(h => h.IdPosologia == p.IdPosologia && h.Hora == hora);

                if (!jaExiste)
                {
                    var h = new Horario();
                    h.Hora = TimeOnly.Parse(horario);
                    h.IdPosologia = p.IdPosologia;
                    h.Posologia = p;

                    _context.Horarios.Add(h);
                    await _context.SaveChangesAsync();
                }
            }
        }

        private async Task PostAlarme(Posologia p, DateTime dtHora)
        {
            var ultimoId = await _context.Alarmes.OrderByDescending(x => x.IdAlarme).Select(x => x.IdAlarme).FirstOrDefaultAsync();

            var a = new Alarme();
            a.IdAlarme = ultimoId + 1;
            a.IdPosologia = p.IdPosologia;
            a.Descricao = $"Tomar {p.Quantidade} {GetTipoFarmaceutico(p.IdTipoFarmaceutico)} de {p.QuantidadeDose} {GetTipoGrandeza(p.IdTipoGrandeza)} de {GetNomeRemedio(p.IdRemedio)}";
            a.DataHora = dtHora;
            a.Status = "A";
            a.Posologia = p;

            _context.Alarmes.Add(a);
            await _context.SaveChangesAsync();

        }

        private async Task AgendamentoPorHora(Posologia p)
        {
            var horarios = await _context.Horarios.Where(h => h.IdPosologia == p.IdPosologia).ToListAsync();

            DateTime dataAtual = p.DataInicio;
            DateTime dataFim = p.DataFim != default ? p.DataFim : DateTime.Now.AddMonths(1);

            while (dataAtual <= dataFim)
            {
                foreach (var horario in horarios)
                {
                    var dataHora = dataAtual.Date.Add(horario.Hora.ToTimeSpan());
                    if (dataHora > p.DataFim)
                        continue;
                    await PostAlarme(p, dataHora);
                }
                dataAtual = dataAtual.AddDays(1);
            }
        }

        private async Task AgendamentoPorDiaSemana(Posologia p)
        {
            var horarios = await _context.Horarios.Where(h => h.IdPosologia == p.IdPosologia).ToListAsync();
            DateTime dataAtual = p.DataInicio;
            DateTime dataFim = p.DataFim != default ? p.DataFim : DateTime.Now.AddMonths(1);

            while (dataAtual <= dataFim)
            {
                var diaAtual = dataAtual.DayOfWeek;

                // Converte o valor numérico de DayOfWeek para o nome do dia da semana em português
                string diaAtualNome = diaAtual switch
                {
                    DayOfWeek.Sunday => "Domingo",
                    DayOfWeek.Monday => "Segunda",
                    DayOfWeek.Tuesday => "Terça",
                    DayOfWeek.Wednesday => "Quarta",
                    DayOfWeek.Thursday => "Quinta",
                    DayOfWeek.Friday => "Sexta",
                    DayOfWeek.Saturday => "Sábado",
                    _ => throw new InvalidOperationException("Dia inválido")
                };

                if (p.DiasSemana.Contains(diaAtualNome))
                {
                    foreach (var horario in horarios)
                    {
                        var dataHora = dataAtual.Date.Add(horario.Hora.ToTimeSpan());  // Atribui a hora do alarme
                        if (dataHora > p.DataFim)
                            continue;
                        await PostAlarme(p, dataHora);
                    }
                    dataAtual = dataAtual.AddDays(1);
                }
                else dataAtual = dataAtual.AddDays(1);
            }
        }

        private async Task AgendamentoPorCiclo(Posologia p)
        {
            var horarios = await _context.Horarios.Where(h => h.IdPosologia == p.IdPosologia).ToListAsync();
            DateTime dataAtual = p.DataInicio;
            DateTime dataFim = p.DataFim != default ? p.DataFim : DateTime.Now.AddMonths(1);

            while (dataAtual <= dataFim)
            {
                int numeroDia = (int)dataAtual.DayOfWeek;
                for (int i = 0; i < p.DiasUso; i++)
                {
                    foreach (var horario in horarios)
                    {
                        var dataHora = dataAtual.Date.Add(horario.Hora.ToTimeSpan());
                        if (dataHora > p.DataFim)
                            continue;
                        await PostAlarme(p, dataHora);
                    }
                    dataAtual = dataAtual.AddDays(1);
                }
                for (int i = 0; i < p.DiasPausa; i++) dataAtual = dataAtual.AddDays(1);
            }
        }
        
        
        #endregion
    }
}
