using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models
{
    public class SolicitacoesVinculo
    {
        [Key]
        public int IdSolicitacao { get; set; }
        public int IdSolicitante { get; set; }
        public int IdTipoSolicitante { get; set; }
        public int IdReceptor { get; set; }
        public int IdTipoReceptor { get; set; } 
        public string Status { get; set; }       
        public DateTime DataSolicitacao { get; set; }
        public Utilizador Solicitante { get; set; }        
        public Utilizador Receptor { get; set; }    
        public UtilizadorTipoUtilizador TipoSolicitante { get; set; }
        public UtilizadorTipoUtilizador TipoReceptor { get; set; }

    }
}