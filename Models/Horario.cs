using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models
{
    public class Horario
    {
        public int IdHorario { get; set; }
        public int IdPosologia { get; set; }
        public DateTime Hora { get; set; }

        public Posologia? Posologia { get; set; }
    }

}