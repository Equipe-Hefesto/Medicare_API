using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Medicare_API.Data;
using Medicare_API.Models;
using Medicare_API.Models.DTOs;
using Medicare_API.Models.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace Medicare_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UtilizadorController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        private readonly IEmailSender _emailSender;

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _context.Utilizadores.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                return Ok(); // Evita revelar se o email existe

            // Gera token e define validade (30m)
            var token = Guid.NewGuid().ToString();
            var expiration = DateTime.UtcNow.AddMinutes(30);

            user.PasswordResetToken = token;
            user.PasswordResetTokenExpiration = expiration;

            await _context.SaveChangesAsync();

            var webLink = $"https://equipe-hefesto.github.io/reset.html?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(token)}&expirration={Uri.EscapeDataString(expiration.ToString("o"))}";

            var message = $@"
                <p>Você solicitou a redefinição de senha.</p>
                <p>Clique no botão abaixo para abrir o app:</p>
                <a href='{webLink}' style='
                display: inline-block;
                padding: 12px 24px;
                background-color: #2e86de;
                co  lor: white;
                text-decoration: none;
                border-radius: 6px;
                font-weight: bold;'>Redefinir Senha</a>
                <p>Se você não solicitou, ignore este e-mail.</p>";

            await _emailSender.SendEmailAsync(user.Email, "Redefinição de senha", message);

            return Ok();
        }


        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _context.Utilizadores.FirstOrDefaultAsync(u =>
                u.Email == request.Email &&
                u.PasswordResetToken == request.Token &&
                u.PasswordResetTokenExpiration > DateTime.UtcNow);

            if (user == null)
                return BadRequest("Token inválido ou expirado.");

            // Cria salt e hash da nova senha
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            user.SenhaSalt = hmac.Key;
            user.SenhaHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.NewPassword));

            // Invalida o token
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiration = null;

            await _context.SaveChangesAsync();

            return Ok("Senha redefinida com sucesso.");
        }




        public UtilizadorController(DataContext context, IConfiguration configuration, IEmailSender emailSender)
        {
            _context = context;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        #region GET

        [HttpGet("GetAll")]
        [Authorize(Roles = "ADMIN")]
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

        [HttpGet("GetSingle")]

        public async Task<ActionResult<Utilizador>> GetUtilizador()
        {

            var userString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userString, out int userId))
            {
                return Unauthorized("Token invalido");
            }

            var utilizadores = await _context.Utilizadores.Where(p => p.IdUtilizador == userId).ToListAsync();
            if (utilizadores == null)
                return NotFound($"O Utilizador com o ID {userId} não foi encontrado.");

            try
            {
                var utilizador = await _context.Utilizadores.FindAsync(userId);
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
        [AllowAnonymous]
        [HttpPost("SingUp")]
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
                u.Username = dto.Username;
                u.SenhaHash = hash;
                u.SenhaSalt = salt;

                _context.Utilizadores.Add(u);
                await _context.SaveChangesAsync();

                var ut = new UtilizadorTipoUtilizador();
                ut.IdUtilizador = u.IdUtilizador;
                ut.IdTipoUtilizador = 2;

                _context.UtilizadoresTiposUtilizadores.Add(ut);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUtilizador), new { id = u.IdUtilizador }, u);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar item: {ex.Message}");
            }
        }
        #endregion
        #region POST2
        [AllowAnonymous]
        [HttpPost("SingUp2")]
        public async Task<ActionResult> PostUtilizador2([FromBody] UtilizadorCreateDTO dto)
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
                u.Username = dto.Username;
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
        [HttpPut("Update")]
        public async Task<ActionResult> PutAsync([FromBody] UtilizadorUpdateDTO dto)
        {
            try
            {
                // Verifica se o usuário autenticado é o mesmo do ID informado
                var userString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userString, out int userId))
                    return Unauthorized("Token inválido.");

                if (dto.IdUtilizador != userId)
                    return Forbid("Você não pode alterar dados de outro usuário.");

                // Busca o usuário a ser atualizado
                var utilizador = await _context.Utilizadores.FindAsync(userId);
                if (utilizador == null)
                    return NotFound($"O Utilizador com o ID {userId} não foi encontrado.");

                // Atualiza os campos
                utilizador.CPF = dto.CPF;
                utilizador.Nome = dto.Nome;
                utilizador.Sobrenome = dto.Sobrenome;
                utilizador.DtNascimento = dto.DtNascimento;
                utilizador.Email = dto.Email;
                utilizador.Telefone = dto.Telefone;
                utilizador.Username = dto.Username;

                // Atualiza a senha (caso venha uma nova senha no DTO)
                if (!string.IsNullOrWhiteSpace(dto.SenhaString))
                {
                    Criptografia.CriarPasswordHash(dto.SenhaString, out byte[] hash, out byte[] salt);
                    utilizador.SenhaHash = hash;
                    utilizador.SenhaSalt = salt;
                }

                _context.Utilizadores.Update(utilizador);
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

        [HttpDelete("Delete")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> DeleteUtilizador()
        {
            try
            {

                var userString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userString, out int userId))
                {
                    return Unauthorized("Token Invalido");
                }

                var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(x => x.IdUtilizador == userId);
                if (utilizador == null)
                    return NotFound($"O Utilizador com o ID {userId} não foi encontrado.");

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

        #region Autenticar
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> AutenticarUsuario(UtilizadorAutenticarDTO credenciais)

        {
            try
            {
                Utilizador? utilizador = await _context.Utilizadores
                   .FirstOrDefaultAsync(x => x.Username.Equals(credenciais.Username) || x.Email.Equals(credenciais.Email));

                if (utilizador == null)
                {
                    throw new System.Exception("Usuário não encontrado.");
                }
                else
                {
                    if (!Criptografia.VerificarPasswordHash(credenciais.SenhaString, utilizador.SenhaHash!, utilizador.SenhaSalt!))
                    {
                        throw new UnauthorizedAccessException("Senha incorreta.");
                    }
                    else
                    {

                        var utilizador1 = _context.Utilizadores
                       .Include(u => u.TiposUtilizadores)
                       .ThenInclude(ut => ut.TipoUtilizador)
                       .FirstOrDefault(u => u.Username == credenciais.Username);

                        utilizador.SenhaHash = null;
                        utilizador.SenhaSalt = null;
                        utilizador.Token = CreateToken(utilizador);


                        return Ok(utilizador);
                    }
                }

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message + " - " + ex.InnerException);
            }

        }
        #endregion
        #region  Token
        private string CreateToken(Utilizador utilizador)
        {

            var chaveSecreta = _configuration.GetValue<string>("ConfiguracaoToken:Chave");
            TipoUtilizador tipoUtilizador = new TipoUtilizador();
            if (string.IsNullOrEmpty(chaveSecreta))
            {
                throw new InvalidCastException("Geração de Token Inválida, se autentique outra vez");
            }

            var claims = new List<Claim>{

            new Claim(ClaimTypes.NameIdentifier, utilizador.IdUtilizador.ToString()),
            new Claim(ClaimTypes.Name, utilizador.Username),


        };
            foreach (var tipo in utilizador.TiposUtilizadores.Select(ut => ut.TipoUtilizador!.Descricao))
            {
                claims.Add(new Claim(ClaimTypes.Role, tipo));
            }

            // Crie a chave de segurança
            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveSecreta));

            // Defina as credenciais de assinatura usando o algoritmo HmacSha256   
            var cred = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            //Defina a data de expiração do token
            var expiracao = DateTime.UtcNow.AddDays(1);

            // Crie o descritor do token com as claims, expiração e credenciais
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiracao,
                SigningCredentials = cred,
                IssuedAt = DateTime.UtcNow, // Registra quando o token foi emitido
                Issuer = _configuration["ConfiguracaoToken:Issuer"],  // Se necessário, adicione o emissor
                Audience = _configuration["ConfiguracaoToken:Audience"] // Se necessário, adicione o público
            };

            // Crie o token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Retorne o token JWT como string
            return tokenHandler.WriteToken(token);
        }

        #endregion


        #region ValidarCpf

        [AllowAnonymous]
        [HttpPost("validar-cpf")]
        public async Task<IActionResult> ValidarCpf([FromBody] ValidarCpfDTO dto)
        {
            try
            {
                bool existe = await _context.Utilizadores
                    .AnyAsync(x => x.CPF.ToLower() == dto.CPF.ToLower());

                return Ok(new { existe });
            }
            catch (Exception ex)
            {
                var mensagemErro = ex.InnerException != null
                    ? $"{ex.Message} - {ex.InnerException.Message}"
                    : ex.Message;

                return BadRequest(new { erro = mensagemErro });
            }
        }


        #endregion

        #region ValidarEmail

        [AllowAnonymous]
        [HttpPost("validar-email")]
        public async Task<IActionResult> ValidarEmail([FromBody] ValidarEmailDTO dto)
        {
            try
            {
                bool existe = await _context.Utilizadores
                    .AnyAsync(x => x.Email.ToLower() == dto.Email.ToLower());

                return Ok(new { existe });
            }
            catch (Exception ex)
            {
                var mensagemErro = ex.InnerException != null
                    ? $"{ex.Message} - {ex.InnerException.Message}"
                    : ex.Message;

                return BadRequest(new { erro = mensagemErro });
            }
        }


        #endregion

    }
}
