using Medicare_API.Data;
using Medicare_API.Models;
using Medicare_API.Models.DTOs;
using Medicare_API.Models.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Medicare_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UtilizadorController : ControllerBase
    {
        private readonly DataContext _context;

        public UtilizadorController(DataContext context)
        {
            _context = context;
        }

        #region GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utilizador>>> ListarUtilizadores()
        {
            try
            {
                var utilizadores = await _context.Utilizadores.ToListAsync();
                if (utilizadores == null || utilizadores.Count == 0)
                {
                    return NotFound();
                }

                return Ok(utilizadores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar utilizadores: {ex.Message}");
            }
        }
        #endregion

        #region GET {id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Utilizador>> GetUtilizador(int id)
        {
            try
            {
                var utilizador = await _context.Utilizadores.FindAsync(id);
                if (utilizador == null)
                {
                    return NotFound();
                }

                return Ok(utilizador);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar utilizador: {ex.Message}");
            }
        }
        #endregion

        #region POST
        [HttpPost]
        public async Task<ActionResult> PostUtilizador([FromBody] UtilizadorCreateDTO dto)
        {
            try
            {
                if (await _context.Utilizadores.AnyAsync(t => t.CPF == dto.CPF))
                {
                    return BadRequest($"O CPF {dto.CPF} informado já existe.");
                }

                //Validar informações

                var ultimoId = await _context.Utilizadores.OrderByDescending(x => x.IdUtilizador).Select(x => x.IdUtilizador).FirstOrDefaultAsync();

                Criptografia.CriarPasswordHash(dto.SenhaString, out byte[] hash, out byte[] salt);
                
                var u = new Utilizador();

                u.IdUtilizador = ultimoId + 1;
                u.CPF = dto.CPF;
                u.Nome = dto.Nome;
                u.Sobrenome = dto.Sobrenome;
                u.DtNascimento = dto.DtNascimento;
                u.Email = dto.Email;
                u.Telefone = dto.Telefone;
                u.SenhaHash = hash;
                u.SenhaSalt = salt;

                _context.Utilizadores.Add(u);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUtilizador), new { id = u.IdUtilizador }, u);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar item: {ex.Message}");
            }
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] UtilizadorUpdateDTO dto)
        {
            try
            {
                if (!await _context.Utilizadores.AnyAsync(x => x.IdUtilizador == id))
                    return NotFound($"O Utilizador com o ID {id} não foi encontrado.");

                Criptografia.CriarPasswordHash(dto.SenhaString, out byte[] hash, out byte[] salt);
                
                var u = new Utilizador();

                u.CPF = dto.CPF;
                u.Nome = dto.Nome;
                u.Sobrenome = dto.Sobrenome;
                u.DtNascimento = dto.DtNascimento;
                u.Email = dto.Email;
                u.Telefone = dto.Telefone;
                u.SenhaHash = hash;
                u.SenhaSalt = salt;

                _context.Utilizadores.Update(u);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar Utilizador: {ex.Message}");
            }
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUtilizador(int id)
        {
            try
            {
                var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(x => x.IdUtilizador == id);
                if (utilizador == null)
                    return NotFound($"O Utilizador com o ID {id} não foi encontrado.");

                _context.Utilizadores.Remove(utilizador);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar Utilizador: {ex.Message}");
            }
        }
        #endregion
    }
}
