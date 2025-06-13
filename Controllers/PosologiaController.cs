using System.Security.Claims;
using System.Xml;
using Medicare_API.Data;
using Medicare_API.Models;
using Medicare_API.Models.DTOs;
using Medicare_API.Models.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medicare_API.Controllers
{
    [Authorize]
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
        [Authorize(Roles = "ADMIN")]
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

        #region GET por Id
        [Authorize(Roles = "ADMIN")]
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

        #region GET por Utilizador 
        [HttpGet("utilizador")]
        public async Task<ActionResult<IEnumerable<Posologia>>> GetPosologiaPorUtilizador()
        {
            try
            {
                var userString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userString, out int userId))
                {
                    return Unauthorized();
                }

                var posologia = await (
                                from p in _context.Posologias
                                where p.IdUtilizador == userId
                                select p
                                ).ToListAsync();

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
        public async Task<ActionResult> PostPosologia([FromBody] PosologiaDTO dto)
        {
            try
            {
                var userString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userString, out int userId)) return Unauthorized();

                if (await ExistePosologiaIdentica(dto, userId))
                {
                    return BadRequest("Já existe uma posologia idêntica cadastrada.");
                }

                var p = new Posologia
                {
                    IdRemedio = dto.IdRemedio,
                    IdUtilizador = userId,
                    IdTipoFarmaceutico = dto.IdTipoFarmaceutico,
                    IdTipoGrandeza = dto.IdTipoGrandeza,
                    IdTipoAgendamento = dto.IdTipoAgendamento,
                    QtdeDose = dto.QtdeDose,
                    QtdeConcentracao = dto.QtdeConcentracao,
                    DataInicio = dto.DataInicio,
                    DataFim = dto.DataFim,
                    Intervalo = dto.Intervalo!,
                    DiasSemana = dto.DiasSemana!,
                    DiasUso = dto.DiasUso,
                    DiasPausa = dto.DiasPausa
                };

                _context.Posologias.Add(p);
                await _context.SaveChangesAsync();

                await TriggerPosologia(p, dto.Horarios);

                return CreatedAtAction(nameof(GetPosologiaPorId), new { id = p.IdPosologia }, p);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar item: {ex.Message}");
            }
        }
        #endregion

        #region PUT
        [HttpPut("{idPosologia}")]
        public async Task<ActionResult> PutPosologia(int idPosologia, [FromBody] PosologiaDTO dto)
        {
            try
            {
                var userString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userString, out int userId))
                    return Unauthorized();

                var p = await _context.Posologias.FirstOrDefaultAsync(p => p.IdPosologia == idPosologia && p.IdUtilizador == userId);
                if (p == null)
                    return NotFound($"Nenhuma posologia encontrada com ID {idPosologia} para o usuário atual.");

                // Verifica se já existe uma posologia idêntica com os dados atualizados (excluindo a própria posologia atual)
                if (await ExistePosologiaIdentica(dto, userId))
                {
                    return BadRequest("Já existe uma posologia idêntica cadastrada.");
                }

                // Atualiza somente os campos que vieram preenchidos no DTO
                if (dto.IdRemedio != default && dto.IdRemedio != p.IdRemedio)
                    p.IdRemedio = dto.IdRemedio;

                if (dto.QtdeDose != default && dto.QtdeDose != p.QtdeDose)
                    p.QtdeDose = dto.QtdeDose;

                if (dto.IdTipoFarmaceutico != default && dto.IdTipoFarmaceutico != p.IdTipoFarmaceutico)
                    p.IdTipoFarmaceutico = dto.IdTipoFarmaceutico;

                if (dto.QtdeConcentracao != default && dto.QtdeConcentracao != p.QtdeConcentracao)
                    p.QtdeConcentracao = dto.QtdeConcentracao;

                if (dto.IdTipoGrandeza != default && dto.IdTipoGrandeza != p.IdTipoGrandeza)
                    p.IdTipoGrandeza = dto.IdTipoGrandeza;

                if (dto.IdTipoAgendamento != default && dto.IdTipoAgendamento != p.IdTipoAgendamento)
                    p.IdTipoAgendamento = dto.IdTipoAgendamento;

                if (dto.DataInicio != default && dto.DataInicio != p.DataInicio)
                    p.DataInicio = dto.DataInicio;

                if (dto.DataFim != default && dto.DataFim != p.DataFim)
                    p.DataFim = dto.DataFim;

                if (!string.IsNullOrEmpty(dto.Intervalo) && dto.Intervalo != p.Intervalo)
                    p.Intervalo = dto.Intervalo;

                if (dto.DiasSemana != null && dto.DiasSemana.Any() && !dto.DiasSemana.SequenceEqual(p.DiasSemana))
                    p.DiasSemana = dto.DiasSemana;

                if (dto.DiasUso != default && dto.DiasUso != p.DiasUso)
                    p.DiasUso = dto.DiasUso;

                if (dto.DiasPausa != default && dto.DiasPausa != p.DiasPausa)
                    p.DiasPausa = dto.DiasPausa;

                // Aqui pode incluir atualização para outras propriedades opcionais como observações, se existirem no DTO

                // Atualiza a posologia no banco
                _context.Posologias.Update(p);
                await _context.SaveChangesAsync();

                // Remove horários e alarmes antigos
                var horarios = await _context.Horarios.Where(h => h.IdPosologia == p.IdPosologia).ToListAsync();
                var alarmes = await _context.Alarmes.Where(a => a.IdPosologia == p.IdPosologia && a.DataHora > DateTime.Now).ToListAsync();

                _context.Horarios.RemoveRange(horarios);
                _context.Alarmes.RemoveRange(alarmes);
                await _context.SaveChangesAsync();

                // Dispara o trigger para recriar os horários e alarmes com base na nova configuração
                await TriggerPosologia(p, dto.Horarios);

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
        [Authorize(Roles = "ADMIN")]
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


        #region POST Horarios
        private async Task PostHorarios(Posologia p, List<string> lista)
        {
            List<string> horarios = new();

            if (lista == null || p.IdTipoAgendamento == 2)
            {
                if (!int.TryParse(p.Intervalo, out int horas) || horas <= 0)
                    throw new ArgumentException("Intervalo inválido.");

                DateTime dtAtual = p.DataInicio.Date;
                var intervaloHoras = TimeSpan.FromHours(horas);

                while (dtAtual < p.DataInicio.Date.AddDays(1))
                {
                    horarios.Add(TimeOnly.FromDateTime(dtAtual).ToString("HH:mm"));
                    dtAtual = dtAtual.Add(intervaloHoras);
                }
            }
            else
            {
                horarios = lista;
            }

            // Normaliza os horários (evita erro de cultura ou formatação)
            var horariosConvertidos = horarios
                .Select(h => TimeOnly.Parse(h.Trim()))
                .Distinct();

            var existentes = await _context.Horarios
                .Where(h => h.IdPosologia == p.IdPosologia)
                .Select(h => h.Hora)
                .ToListAsync();

            var novosHorarios = horariosConvertidos
                .Where(h => !existentes.Contains(h))
                .Select(h => new Horario
                {
                    Hora = h,
                    IdPosologia = p.IdPosologia,
                })
                .ToList();

            if (novosHorarios.Any())
            {
                _context.Horarios.AddRange(novosHorarios);
                await _context.SaveChangesAsync();
            }
        }
        #endregion

        #region POST Alarme
        private async Task PostAlarme(Posologia p, DateTime dtHora)
        {
            var a = new Alarme
            {
                IdPosologia = p.IdPosologia,
                DataHora = dtHora,
            };

            _context.Alarmes.Add(a);
            await _context.SaveChangesAsync();

        }
        #endregion

        #region Criação agendamentos
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
            var horarios = await _context.Horarios
                .Where(h => h.IdPosologia == p.IdPosologia)
                .ToListAsync();

            DateTime dataAtual = p.DataInicio;
            DateTime dataFim = p.DataFim != default ? p.DataFim : DateTime.Now.AddMonths(1);

            while (dataAtual <= dataFim)
            {
                DayOfWeek diaAtual = dataAtual.DayOfWeek;
                if (p.DiasSemana.Contains(diaAtual))
                {
                    foreach (var horario in horarios)
                    {
                        var dataHora = dataAtual.Date.Add(horario.Hora.ToTimeSpan());
                        if (dataHora > p.DataFim)
                            continue;
                        await PostAlarme(p, dataHora);
                    }
                }

                dataAtual = dataAtual.AddDays(1);
            }
        }

        private async Task AgendamentoPorCiclo(Posologia p)
        {
            var horarios = await _context.Horarios
                .Where(h => h.IdPosologia == p.IdPosologia)
                .ToListAsync();

            DateTime dataAtual = p.DataInicio;
            DateTime dataFim = p.DataFim != default ? p.DataFim : DateTime.Now.AddMonths(1);

            while (dataAtual <= dataFim)
            {
                // Dias de uso
                for (int i = 0; i < p.DiasUso && dataAtual <= dataFim; i++)
                {
                    foreach (var horario in horarios)
                    {
                        var dataHora = dataAtual.Date.Add(horario.Hora.ToTimeSpan());

                        if (dataHora > p.DataFim) continue;

                        await PostAlarme(p, dataHora);
                    }

                    dataAtual = dataAtual.AddDays(1);
                }

                // Dias de pausa (sem alarmes)
                for (int i = 0; i < p.DiasPausa && dataAtual <= dataFim; i++)
                {
                    dataAtual = dataAtual.AddDays(1);
                }
            }
        }
        #endregion

        #region Adicionais
        private async Task<bool> ExistePosologiaIdentica(PosologiaDTO dto, int userId)
        {
            var existe = await _context.Posologias
                .Where(p =>
                    p.IdRemedio == dto.IdRemedio &&
                    p.IdUtilizador == userId &&
                    p.QtdeDose == dto.QtdeDose &&
                    p.QtdeConcentracao == dto.QtdeConcentracao &&
                    p.IdTipoFarmaceutico == dto.IdTipoFarmaceutico &&
                    p.IdTipoGrandeza == dto.IdTipoGrandeza &&
                    p.IdTipoAgendamento == dto.IdTipoAgendamento &&
                    p.DataInicio.Date == dto.DataInicio.Date &&
                    p.DataFim.Date == dto.DataFim.Date &&
                    p.Intervalo == dto.Intervalo &&
                    p.DiasUso == dto.DiasUso &&
                    p.DiasPausa == dto.DiasPausa
                )
                .Include(p => p.Horarios)
                .Include(p => p.DiasSemana)
                .FirstOrDefaultAsync();

            if (existe == null)
                return false;

            // Verifica se os horários e dias são idênticos
            var horariosDb = existe.Horarios.Select(h => h.Hora.ToString("HH:mm")).OrderBy(h => h);
            var horariosDto = dto.Horarios.OrderBy(h => h);

            var diasDb = existe.DiasSemana.OrderBy(d => d);
            var diasDto = dto.DiasSemana.OrderBy(d => d);

            return horariosDb.SequenceEqual(horariosDto) && diasDb.SequenceEqual(diasDto);
        }

        private async Task TriggerPosologia(Posologia p, List<string> horarios)
        {
            var soneca = new Soneca
            {
                IdPosologia = p.IdPosologia,
            };

            _context.Sonecas.Add(soneca);
            await _context.SaveChangesAsync();

            await PostHorarios(p, horarios);

            switch (p.IdTipoAgendamento)
            {
                case 1:
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
        }
        #endregion
    }
}