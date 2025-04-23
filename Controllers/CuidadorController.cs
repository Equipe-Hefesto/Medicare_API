using Medicare_API.Data;
using Medicare_API.Models;
using Medicare_API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medicare_API.Controllers
{
    [Route("[controller]")]
    public class CuidadorController : Controller
    {
        private readonly DataContext _context;

        public CuidadorController(DataContext context)
        {
            _context = context;
        }

        #region GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cuidador>>> GetAllRelacionamentos()
        {
            try
            {
                var relacionamentos = await _context.Cuidadores.ToListAsync();
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
        [HttpGet("{idCuidador}/{idPaciente}")]
        public async Task<ActionResult<Cuidador>> GetRelacionamentoPorIds(int idCuidador, int idPaciente)
        {
            try
            {
                var relacionamento = await _context.Cuidadores.FirstOrDefaultAsync(c => c.IdCuidador == idCuidador && c.IdPaciente == idPaciente);
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
        [HttpPost]
        public async Task<ActionResult> PostRelacionamento([FromBody] CuidadorDTO dto)
        {
            try
            {
                // Validar se o Cuidador existe e é do tipo correto
                (ActionResult? erroCuidador, Utilizador? cuidador) = await ObterUtilizadorValidado(dto.IdCuidador, 2, "Cuidador");
                if (erroCuidador != null) return erroCuidador;  // Se houver erro, retorna o erro imediatamente

                // Validar se o Paciente existe e é do tipo correto
                (ActionResult? erroPaciente, Utilizador? paciente) = await ObterUtilizadorValidado(dto.IdPaciente, 1, "Paciente");
                if (erroPaciente != null) return erroPaciente;  // Se houver erro, retorna o erro imediatamente

                // Validar se o relacionamento já existe
                (ActionResult? erroRelacao, Cuidador? relação) = await VerificarRelacionamento(dto.IdCuidador, dto.IdPaciente, "nao_existe");
                if (erroRelacao != null) return erroRelacao;
                //Validar informações


                var c = new Cuidador();
                c.IdCuidador = dto.IdCuidador;
                c.IdPaciente = dto.IdPaciente;
                c.DataInicio = dto.DataInicio;
                c.DataFim = dto.DataFim;
                c.DataCriacao = DateTime.Now;
                c.DataAtualizacao = DateTime.Now;
                c.Status = "A";
                c.CuidadorUser = cuidador;
                c.Paciente = paciente;

                _context.Cuidadores.Add(c);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRelacionamentoPorIds), new { idCuidador = c.IdCuidador, idPaciente = c.IdPaciente }, c);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar item: {ex.Message}");
            }
        }
        #endregion

        #region PUT
        [HttpPut("{idCuidador}/{idPaciente}")]
        public async Task<ActionResult> PutRelacionamento(int idCuidador, int idPaciente, [FromBody] CuidadorUpdateDTO dto)
        {
            try
            {   // Validar se o Cuidador existe e é do tipo correto
                (ActionResult? erroCuidador, Utilizador? cuidador) = await ObterUtilizadorValidado(idCuidador, 2, "Cuidador");
                if (erroCuidador != null) return erroCuidador;  // Se houver erro, retorna o erro imediatamente

                // Validar se o Paciente existe e é do tipo correto
                (ActionResult? erroPaciente, Utilizador? paciente) = await ObterUtilizadorValidado(idPaciente, 1, "Paciente");
                if (erroPaciente != null) return erroPaciente;  // Se houver erro, retorna o erro imediatamente

                // Validar se o relacionamento já existe
                (ActionResult? erroRelacao, Cuidador? relacionamento ) = await VerificarRelacionamento(dto.IdCuidador, dto.IdPaciente, "existe");
                if (erroRelacao != null) return erroRelacao;  // Se houver erro, retorna o erro imediatamente

                var c = relacionamento !;

                c.IdCuidador = dto.IdCuidador;
                c.IdPaciente = dto.IdPaciente;
                c.DataInicio = dto.DataInicio;
                c.DataFim = dto.DataFim;
                c.DataAtualizacao = DateTime.Now;
                c.Status = dto.Status;
                c.CuidadorUser = cuidador;
                c.Paciente = paciente;

                _context.Cuidadores.Update(c);
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
        [HttpDelete("{idCuidador}/{idPaciente}")]
        public async Task<ActionResult> DeleteCuidador(int idCuidador, int idPaciente)
        {
            try
            {
                // Validar se o relacionamento já existe
                (ActionResult? erroRelacao, Cuidador? relacionamento ) = await VerificarRelacionamento(idCuidador, idPaciente, "existe");
                if (erroRelacao != null) return erroRelacao;  // Se houver erro, retorna o erro imediatamente

                _context.Cuidadores.Remove(relacionamento!);
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
            // Verifica se o usuário existe
            var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.IdUtilizador == id);
            if (utilizador == null)
                return (BadRequest($"O Id{papel} {id} informado não existe."), null);

            // Verifica se o usuário tem o tipo correto
            var tipoValido = await _context.UtilizadoresTiposUtilizadores
                .AnyAsync(ut => ut.IdUtilizador == id && ut.IdTipoUtilizador == tipoEsperado);

            if (!tipoValido)
                return (BadRequest($"O Id{papel} {id} informado não é do tipo certo."), null);

            // Se tudo estiver certo, retorna o usuário
            return (null, utilizador);
        }

        private async Task<(ActionResult? Erro, Cuidador? Relacionamento)> VerificarRelacionamento(int idCuidador, int idPaciente, string tipoValidacao)
        {
            var relacionamento = await _context.Cuidadores
                .FirstOrDefaultAsync(c => c.IdCuidador == idCuidador && c.IdPaciente == idPaciente);

            switch (tipoValidacao.ToLower()) // Convertendo para minúsculo só por segurança
            {
                case "nao_existe":
                    // Se a relação já existe, dá erro. Senão, tudo certo (pode seguir e criar).
                    if (relacionamento != null)
                        return (BadRequest($"A relação IdCuidador {idCuidador} + IdPaciente {idPaciente} já existe."), null);
                    return (null, null);

                case "existe":
                    // Se não existe, dá erro. Se existe, retorna ela.
                    if (relacionamento == null)
                        return (BadRequest($"A relação IdCuidador {idCuidador} + IdPaciente {idPaciente} não existe."), null);
                    return (null, relacionamento);

                default:
                    // Se mandarem um valor inválido, dá erro também.
                    return (BadRequest("Tipo de validação inválido. Use 'existe' ou 'nao_existe'."), null);
            }
        }

        #endregion

    }
}