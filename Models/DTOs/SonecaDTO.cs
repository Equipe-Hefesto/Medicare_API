using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models.DTOs
{
    public class SonecaCreateDTO
    {
        public int IdPosologia { get; set; }
        public char StSoneca { get; set; } = 'A';
        public int IntervaloMinutos { get; set; } = 5;

        public int MaxSoneca { get; set; } = 3;
        public DateTime DcSoneca { get; set; } = DateTime.Now;
        public DateTime DuSoneca { get; set; } = DateTime.Now;

    }

    public class SonecaUpdateDTO
    {
        public int IdPosologia { get; set; }
        public char StSoneca { get; set; }
        public int IntervaloMinutos { get; set; } = 5;
        public int MaxSoneca { get; set; }
        public DateTime DuSoneca { get; set; } = DateTime.Now;

    }

    public class SonecaStatusUpdateDTO
    {
        public char StSoneca { get; set; } = 'A';
    }

    
}