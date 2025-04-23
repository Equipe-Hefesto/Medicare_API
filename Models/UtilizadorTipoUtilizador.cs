using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models
{
    public class UtilizadorTipoUtilizador
    {
        public int IdUtilizador { get; set; }
        public int IdTipoUtilizador { get; set; }

        public Utilizador? Utilizador { get; set; }
        public TipoUtilizador? TipoUtilizador { get; set; }
    }
}