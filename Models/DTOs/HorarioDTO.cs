using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models.DTOs
{
    public class HorarioCreateDTO
    {
        public int IdPosologia { get; set; }
        public TimeOnly Hora { get; set; }
    }

    public class HorarioUpdateDTO
    {
        public int IdHorario { get; set; }
        public int IdPosologia { get; set; }
        public required TimeOnly Hora { get; set; }
    }

}