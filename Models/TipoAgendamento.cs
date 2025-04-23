using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models
{
    public class TipoAgendamento
    {
        public int IdTipoAgendamento { get; set; }
        public string Descricao { get; set; } = null!;
    }

}