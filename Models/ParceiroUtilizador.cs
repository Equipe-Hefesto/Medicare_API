using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models
{
    public class ParceiroUtilizador
    {
        public int IdParceiro { get; set; }
        public int IdUtilizador { get; set; }

        public Parceiro? Parceiro { get; set; }
        public Utilizador? Utilizador { get; set; }
    }
}