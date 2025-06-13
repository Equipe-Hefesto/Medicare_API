using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models
{
    public class Soneca
    {
        public int IdPosologia { get; set; }
        public char StSoneca { get; set; } = 'A';
        public int IntervaloMinutos { get; set; } = 5;
        public int MaxSoneca { get; set; } = 3;
        public DateTime DcSoneca { get; set; } = DateTime.Now;
        public DateTime DuSoneca { get; set; } = DateTime.Now;

        // Navegação (relacionamento)
        public Posologia? Posologia { get; set; }
    }

}