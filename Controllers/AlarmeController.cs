using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
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
    public class AlarmeController : Controller
    {
        private readonly DataContext _context;

        public AlarmeController(DataContext context)
        {
            _context = context;
        }

        #region GET All
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<Alarme>>> GetAllAlarmes()
        {
            try
            {
                var alarmes = await _context.Alarmes.ToListAsync();
                if (alarmes == null || alarmes.Count == 0)
                    return NotFound();
                return Ok(alarmes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region GET por Id
        [HttpGet("{id}")]
        public async Task<ActionResult<AlarmeCompletoDTO>> GetAlarmePorId(int id)
        {
            try
            {
                var userId = User.GetUserId();
                if (userId == 0) return Unauthorized();

                var alarme = await (
                                from a in _context.Alarmes
                                join p in _context.Posologias on a.IdPosologia equals p.IdPosologia
                                join r in _context.Remedios on p.IdRemedio equals r.IdRemedio
                                join s in _context.Sonecas on p.IdPosologia equals s.IdPosologia
                                where p.IdUtilizador == userId && a.IdAlarme == id
                                select new AlarmeAgendarDTO
                                {
                                    NomeRemedio = r.Nome,
                                    Alarme = a,
                                    Dose = $"{p.QtdeDose} {GetTipoFarmaceutico(p.IdTipoFarmaceutico)} de {p.QtdeConcentracao} {GetTipoGrandeza(p.IdTipoGrandeza)} de {GetNomeRemedio(p.IdRemedio)}",
                                    Observacao = $"{p.Observacao}",
                                    Soneca = s

                                }).FirstOrDefaultAsync();

                if (alarme == null)
                    return NotFound();
                return Ok(alarme);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region GET lista-agendar
        [HttpGet("  ")]
        public async Task<ActionResult<IEnumerable<Alarme>>> GetListaAgendarAlarme()
        {
            try
            {
                var userId = User.GetUserId();
                if (userId == 0) return Unauthorized();

                var alarme = await (
                                from a in _context.Alarmes
                                join p in _context.Posologias on a.IdPosologia equals p.IdPosologia
                                join r in _context.Remedios on p.IdRemedio equals r.IdRemedio
                                join s in _context.Sonecas on p.IdPosologia equals s.IdPosologia
                                where p.IdUtilizador == userId
                                select new AlarmeAgendarDTO
                                {
                                    NomeRemedio = r.Nome,
                                    Alarme = a,
                                    Dose = $"{p.QtdeDose} {GetTipoFarmaceutico(p.IdTipoFarmaceutico)} de {p.QtdeConcentracao} {GetTipoGrandeza(p.IdTipoGrandeza)} de {GetNomeRemedio(p.IdRemedio)}",
                                    Observacao = $"{p.Observacao}",
                                    Soneca = s

                                }).ToListAsync();

                if (alarme == null)
                    return NotFound();
                return Ok(alarme);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region GET lista-completa
        [HttpGet("lista-completa")]
        public async Task<ActionResult<IEnumerable<Alarme>>> GetListaCompletaAlarme()
        {
            try
            {
                var userId = User.GetUserId();
                if (userId == 0) return Unauthorized();

                var alarme = await (
                                from a in _context.Alarmes
                                join p in _context.Posologias on a.IdPosologia equals p.IdPosologia
                                join r in _context.Remedios on p.IdRemedio equals r.IdRemedio
                                join s in _context.Sonecas on p.IdPosologia equals s.IdPosologia
                                where p.IdUtilizador == userId
                                select new AlarmeCompletoDTO
                                {
                                    NomeRemedio = r.Nome,
                                    Alarme = a,
                                    Dose = $"{p.QtdeDose} {GetTipoFarmaceutico(p.IdTipoFarmaceutico)}",
                                    Concentracao = $"{p.QtdeConcentracao} {GetTipoGrandeza(p.IdTipoGrandeza)}",
                                    Observacao = $"{p.Observacao}",
                                    Soneca = s,
                                    Frequencia = $"{GetTipoAgendamento(p.IdTipoAgendamento)} | {GetHorarios(p.IdPosologia)}",
                                    Proximo = $"{GetProximo(p.IdPosologia)}"
                                }).ToListAsync();

                if (alarme == null)
                    return NotFound();
                return Ok(alarme);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region POST
        [HttpPost]
        public async Task<ActionResult> PostAlarme([FromBody] AlarmeDTO dto)
        {
            try
            {
                var userString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userString, out int userId)) return Unauthorized();

                var posologia = await _context.Posologias.FirstOrDefaultAsync(p => p.IdPosologia == dto.IdPosologia && p.IdUtilizador == userId);
                if (posologia == null)
                {
                    return Forbid("Você não tem permissão para adicionar alarmes a essa posologia.");
                }
                if (await _context.Alarmes.AnyAsync(a => a.IdPosologia == dto.IdPosologia && a.DataHora == dto.DataHora))
                {
                    return BadRequest($"O Alarme da Posologia  {dto.IdPosologia} das {dto.DataHora} informado já existe.");
                }

                var a = new Alarme
                {
                    IdPosologia = dto.IdPosologia,
                    DataHora = dto.DataHora,
                    Status = "P"
                };

                _context.Alarmes.Add(a);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAlarmePorId), new { id = a.IdAlarme }, a);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar item: {ex.Message}");
            }
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAlarme(int id, [FromBody] AlarmeDTO dto)
        {
            try
            {
                var userString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userString, out int userId)) return Unauthorized();

                var posologia = await _context.Posologias.FirstOrDefaultAsync(p => p.IdPosologia == dto.IdPosologia && p.IdUtilizador == userId);
                if (posologia == null)
                {
                    return Forbid("Você não tem permissão para alterar alarmes dessa posologia.");
                }
                var a = await _context.Alarmes.FirstOrDefaultAsync(a => a.IdAlarme == id && a.IdPosologia == dto.IdPosologia);
                if (a == null)
                {
                    return NotFound($"O alarme de ID {id} não foi encontrado.");
                }

                a.IdPosologia = dto.IdPosologia;
                a.DataHora = dto.DataHora;
                a.Status = dto.Status;

                _context.Alarmes.Update(a);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar item: {ex.Message}");
            }
        }
        #endregion

        #region PUT status
        [HttpPut("status/{id}")]
        public async Task<ActionResult> AtualizarStatusAlarme(int id, [FromBody] AlarmeStatusDTO dto)
        {
            try
            {
                var userString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userString, out int userId)) return Unauthorized();

                var a = await _context.Alarmes.Include(a => a.Posologia).FirstOrDefaultAsync(a => a.IdAlarme == id && a.Posologia!.IdUtilizador == userId);
                if (a == null)
                {
                    return NotFound($"O alarme de ID {id} não foi encontrado.");
                }

                a.Status = dto.Status;

                _context.Alarmes.Update(a);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar status: {ex.Message}");
            }
        }
        #endregion

        #region PUT contador
        [HttpPut("  {id}")]
        public async Task<ActionResult> AtualizarContadorAlarme(int id, [FromBody] AlarmeContadorDTO dto)
        {
            try
            {
                var userString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userString, out int userId)) return Unauthorized();

                var a = await _context.Alarmes.Include(a => a.Posologia).FirstOrDefaultAsync(a => a.IdAlarme == id && a.Posologia!.IdUtilizador == userId);
                if (a == null)
                {
                    return NotFound($"O alarme de ID {id} não foi encontrado.");
                }

                a.DataHora = dto.DataHora;
                a.ContadorSoneca = dto.ContadorSoneca;
                a.Status = "S";

                _context.Alarmes.Update(a);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar status: {ex.Message}");
            }
        }
        #endregion


        #region DELETE
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        [Authorize(Roles = "AMIGO_MEDICARE")]
        public async Task<ActionResult> DeleteAlarme(int id)
        {
            try
            {
                var alarme = await _context.Alarmes.FirstOrDefaultAsync(x => x.IdAlarme == id);
                if (alarme == null)
                    return NotFound($"O Alarme com o ID {id} não foi encontrado.");

                _context.Alarmes.Remove(alarme);
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
        private string GetTipoAgendamento(int IdTipoAgendamento)
        {
            return _context.TiposAgendamento.Find(IdTipoAgendamento)?.Descricao ?? "Tipo Agendamento";
        }
        private string GetHorarios(int idPosologia)
        {
            var horarios = _context.Horarios
                .Where(h => h.IdPosologia == idPosologia)
                .Select(h => h.Hora.ToString(@"hh\:mm"))
                .ToList();

            return horarios.Any() ? string.Join(", ", horarios) : "Horarios";
        }
        private async Task<string> GetProximo(int idPosologia)
        {
            var proximo = await _context.Alarmes
                .Where(a => a.IdPosologia == idPosologia && a.DataHora > DateTime.Now)
                .OrderBy(a => a.DataHora)
                .Select(a => a.DataHora)
                .FirstOrDefaultAsync();

            if (proximo == default)
                return "Sem alarmes futuros";

            var hoje = DateTime.Today;
            var amanha = hoje.AddDays(1);
            var cultura = new CultureInfo("pt-BR");

            if (proximo.Date == hoje)
                return $"Hoje, {proximo:HH:mm}";
            else if (proximo.Date == amanha)
                return $"Amanhã, {proximo:HH:mm}";
            else if (proximo < hoje.AddDays(7))
            {
                var diaSemana = cultura.DateTimeFormat.GetDayName(proximo.DayOfWeek);
                // Capitaliza o primeiro caractere (opcional)
                diaSemana = char.ToUpper(diaSemana[0]) + diaSemana.Substring(1);
                return $"{diaSemana}, {proximo:HH:mm}";
            }
            else
                return $"{proximo:dd/MM/yy}, {proximo:HH:mm}";
        }
        #endregion
    }
}