using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medicare_API.Data;
using Medicare_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;


namespace Medicare_API.Controllers
{
    [Authorize]
    [Route("[controller]")]
     public class SolicitacoesVinculoController : Controller
    {
        private readonly DataContext _context;

        public SolicitacoesVinculoController(DataContext context)
        {
            _context = context;
        }

       #region POST
       [HttpPost("AddSol")]
       public async Task<IActionResult> CriarSolicitacao([FromBody] SolicitacoesVinculo solicitacao)
       {
            solicitacao.Status = "Pendente";
            solicitacao.DataSolicitacao = DateTime.Now;

            _context.SolicitacoesVinculos.Add(solicitacao);
            await _context.SaveChangesAsync();

            return Ok(new {mensagem = "Solicitação criada com sucesso!" });
       }
       #endregion

       #region GET
       [HttpGet("GetAll")]
       public async Task<ActionResult<IEnumerable<SolicitacoesVinculo>>> GetAllSolicitacoes()
       {
            return await _context.SolicitacoesVinculos
            .Include(s => s.Solicitante)
            .Include(s => s.Receptor)
            .Include(s => s.IdTipoSolicitante)
            .Include(s => s.TipoReceptor)
            .ToListAsync();
       }
       #endregion

       #region GET
       [HttpGet("{id}")]
       public async Task<ActionResult<SolicitacoesVinculo>> GetById(int id)
       {

            var solicitacao = await _context.SolicitacoesVinculos
                .Include(s => s.Solicitante)
                .Include(s => s.Receptor)
                .Include(s => s.TipoSolicitante)
                .Include(s => s.TipoReceptor)
                    .FirstOrDefaultAsync(s => s.IdSolicitacao == id);

            if(solicitacao == null)
            return NotFound();

            return Ok(solicitacao); 
       }
       #endregion

       #region PUT
       [HttpPut("{id}/status")]
       public async Task<IActionResult> PutStatus(int id, [FromBody] string novoStatus)
       {

        var solicitacao = await _context.SolicitacoesVinculos.FindAsync(id);

        if(solicitacao == null)
        return NotFound();

        solicitacao.Status = novoStatus;
        await _context.SaveChangesAsync();

        return Ok(new {mensagem = $"Status atualizado para {novoStatus}."});
       }
       #endregion

       #region DELETE
       [HttpDelete("{id}")]
       public async Task<IActionResult> DeleteSolicitacao(int id)
       {
            var solicitacao = await _context.SolicitacoesVinculos.FindAsync(id);
            if (solicitacao == null)
            return NotFound();

            _context.SolicitacoesVinculos.Remove(solicitacao);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Solicitação removida com sucesso."});
       }
       #endregion
    }
}