using System.Security.Claims;
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
    public class SonecaController : Controller
    {
        private readonly DataContext _context;

        public SonecaController(DataContext context)
        {
            _context = context;
        }

        #region GET ALL
        [HttpGet("GetAll")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<Soneca>>> GetAllSonecas()
        {
            try
            {
                var sonecas = await _context.Sonecas.ToListAsync();
                if (sonecas == null || !sonecas.Any())
                    return NotFound();

                return Ok(sonecas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region GET por utilizador
        [HttpGet("utilizador")]
        public async Task<ActionResult<IEnumerable<Soneca>>> GetSonecaPorUtilizador()
        {
            try
            {
                var userId = User.GetUserId();
                if (userId == 0) return Unauthorized();

                var sonecas = await (
                    from s in _context.Sonecas
                    join p in _context.Posologias on s.IdPosologia equals p.IdPosologia
                    where p.IdUtilizador == userId
                    select s
                ).ToListAsync();

                if (!sonecas.Any()) return NotFound();

                return Ok(sonecas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        #endregion

        #region POST
        [HttpPost]
        public async Task<ActionResult> PostSoneca([FromBody] SonecaCreateDTO dto)
        {
            try
            {
                var userId = User.GetUserId();
                if (userId == 0) return Unauthorized();

                var posologia = await _context.Posologias.FirstOrDefaultAsync(p => p.IdPosologia == dto.IdPosologia && p.IdUtilizador == userId);
                if (posologia == null)
                    return Forbid("Você não tem permissão para adicionar sonecas a essa posologia.");

                var soneca = new Soneca
                {
                    IdPosologia = dto.IdPosologia,
                    StSoneca = 'A',
                    IntervaloMinutos = dto.IntervaloMinutos,
                    MaxSoneca = dto.MaxSoneca,
                    DcSoneca = DateTime.Now,
                    DuSoneca = DateTime.Now
                };

                _context.Sonecas.Add(soneca);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetSonecaPorUtilizador), new { id = soneca.IdPosologia }, soneca);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar: {ex.Message}");
            }
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public async Task<ActionResult> PutSoneca(int id, [FromBody] SonecaUpdateDTO dto)
        {
            try
            {
                var userId = User.GetUserId();
                if (userId == 0) return Unauthorized();

                var soneca = await _context.Sonecas
                    .Include(s => s.Posologia)
                    .FirstOrDefaultAsync(s => s.IdPosologia == id && s.Posologia!.IdUtilizador == userId);

                if (soneca == null) return NotFound();

                soneca.IntervaloMinutos = dto.IntervaloMinutos;
                soneca.MaxSoneca = dto.MaxSoneca;
                soneca.DuSoneca = DateTime.Now;
                soneca.StSoneca = dto.StSoneca;

                _context.Sonecas.Update(soneca);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar: {ex.Message}");
            }
        }
        #endregion

        #region PUT contador
        [HttpPut("Status/{id}")]
        public async Task<ActionResult> AtualizarStatusSoneca(int id, [FromBody] SonecaStatusUpdateDTO dto)
        {
            try
            {
                var userId = User.GetUserId();
                if (userId == 0) return Unauthorized();

                

                var soneca = await _context.Sonecas
                    .Include(s => s.Posologia)
                    .FirstOrDefaultAsync(s => s.IdPosologia == id && s.Posologia!.IdUtilizador == userId);

                if (soneca == null) return NotFound();

                soneca.StSoneca = dto.StSoneca;
                soneca.DuSoneca = DateTime.Now;

                _context.Sonecas.Update(soneca);
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
        [Authorize(Roles = "ADMIN,AMIGO_MEDICARE")]
        public async Task<ActionResult> DeleteSoneca(int id)
        {
            try
            {
                var soneca = await _context.Sonecas.FirstOrDefaultAsync(s => s.IdPosologia == id);
                if (soneca == null)
                    return NotFound("Soneca não encontrada.");

                _context.Sonecas.Remove(soneca);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar: {ex.Message}");
            }
        }
        #endregion
    }

    
}
