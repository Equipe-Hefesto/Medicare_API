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
    public class ResponsavelController : Controller
    {
        private readonly DataContext _context;

        public ResponsavelController(DataContext context)
        {
            _context = context;
        }

        #region GET
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Responsavel>>> GetAllRelacionamentos()
        {
            try
            {
                var relacionamentos = await _context.Responsaveis.ToListAsync();
                if (relacionamentos == null || relacionamentos.Count == 0)
                {
                    return NotFound();
                }

                return Ok(relacionamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region GET {id}
        [HttpGet("{idResponsavel}/{idPaciente}")]
        public async Task<ActionResult<Responsavel>> GetRelacionamentoPorIds(int idResponsavel, int idPaciente)
        {
            try
            {
                var relacionamento = await _context.Responsaveis.FirstOrDefaultAsync(r => r.IdResponsavel == idResponsavel && r.IdPaciente == idPaciente);
                if (relacionamento == null)
                {
                    return NotFound();
                }

                return Ok(relacionamento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region POST
        [HttpPost("AddRel")]
        public async Task<ActionResult> PostRelacionamento([FromBody] ResponsavelCreateDTO dto)
        {
            try
            {
                (ActionResult? erroResponsavel, Utilizador? responsavel) = await ObterUtilizadorValidado(dto.IdResponsavel, 2, "Responsável");
                if (erroResponsavel != null) return erroResponsavel;

                (ActionResult? erroPaciente, Utilizador? paciente) = await ObterUtilizadorValidado(dto.IdPaciente, 1, "Paciente");
                if (erroPaciente != null) return erroPaciente;

                (ActionResult? erroRelacao, Responsavel? relacao) = await VerificarRelacionamento(dto.IdResponsavel, dto.IdPaciente, "nao_existe");
                if (erroRelacao != null) return erroRelacao;

                var r = new Responsavel();

                r.IdResponsavel = dto.IdResponsavel;
                r.IdPaciente = dto.IdPaciente;
                r.DataCriacao = DateTime.Now;
                r.DataAtualizacao = DateTime.Now;
                r.Status = "A";
                r.ResponsavelUser = responsavel;
                r.Paciente = paciente;


                _context.Responsaveis.Add(r);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRelacionamentoPorIds), new { idResponsavel = r.IdResponsavel, idPaciente = r.IdPaciente }, r);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar item: {ex.Message}");
            }
        }
        #endregion

        #region PUT
        [HttpPut("{idResponsavel}/{idPaciente}")]
        public async Task<ActionResult> PutRelacionamento(int idResponsavel, int idPaciente, [FromBody] ResponsavelUpdateDTO dto)
        {
            try
            {
                (ActionResult? erroResponsavel, Utilizador? responsavel) = await ObterUtilizadorValidado(idResponsavel, 2, "Responsável");
                if (erroResponsavel != null) return erroResponsavel;

                (ActionResult? erroPaciente, Utilizador? paciente) = await ObterUtilizadorValidado(idPaciente, 1, "Paciente");
                if (erroPaciente != null) return erroPaciente;

                (ActionResult? erroRelacao, Responsavel? relacionamento) = await VerificarRelacionamento(dto.IdResponsavel, dto.IdPaciente, "existe");
                if (erroRelacao != null) return erroRelacao;

                var r = relacionamento!;

                r.IdResponsavel = dto.IdResponsavel;
                r.IdPaciente = dto.IdPaciente;
                r.DataAtualizacao = DateTime.Now;
                r.Status = dto.Status;
                r.ResponsavelUser = responsavel;
                r.Paciente = paciente;

                _context.Responsaveis.Update(r);
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
        [HttpDelete("{idResponsavel}/{idPaciente}")]
        public async Task<ActionResult> DeleteRelacionamento(int idResponsavel, int idPaciente)
        {
            try
            {
                (ActionResult? erroRelacao, Responsavel? relacionamento) = await VerificarRelacionamento(idResponsavel, idPaciente, "existe");
                if (erroRelacao != null) return erroRelacao;

                _context.Responsaveis.Remove(relacionamento!);
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
        private async Task<(ActionResult? Erro, Utilizador? User)> ObterUtilizadorValidado(int id, int tipoEsperado, string papel)
        {
            var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.IdUtilizador == id);
            if (utilizador == null)
                return (BadRequest($"O Id{papel} {id} informado não existe."), null);

            var tipoValido = await _context.UtilizadoresTiposUtilizadores
                .AnyAsync(ut => ut.IdUtilizador == id && ut.IdTipoUtilizador == tipoEsperado);

            if (!tipoValido)
                return (BadRequest($"O Id{papel} {id} informado não é do tipo certo."), null);

            return (null, utilizador);
        }

        private async Task<(ActionResult? Erro, Responsavel? Relacionamento)> VerificarRelacionamento(int idResponsavel, int idPaciente, string tipoValidacao)
        {
            var relacionamento = await _context.Responsaveis
                .FirstOrDefaultAsync(r => r.IdResponsavel == idResponsavel && r.IdPaciente == idPaciente);

            switch (tipoValidacao.ToLower())
            {
                case "nao_existe":
                    if (relacionamento != null)
                        return (BadRequest($"A relação IdResponsavel {idResponsavel} + IdPaciente {idPaciente} já existe."), null);
                    return (null, null);

                case "existe":
                    if (relacionamento == null)
                        return (BadRequest($"A relação IdResponsavel {idResponsavel} + IdPaciente {idPaciente} não existe."), null);
                    return (null, relacionamento);

                default:
                    return (BadRequest("Tipo de validação inválido. Use 'existe' ou 'nao_existe'."), null);
            }
        }
        #endregion
    }
}
