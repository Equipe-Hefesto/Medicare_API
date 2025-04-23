using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models.DTOs
{
    public class CuidadorDTO
    {
        public int IdCuidador { get; set; }
        public int IdPaciente { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }

    public class CuidadorUpdateDTO
    {
        public int IdCuidador { get; set; }
        public int IdPaciente { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public required string Status { get; set; }
    }

}