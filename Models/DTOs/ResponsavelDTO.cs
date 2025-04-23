using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models.DTOs
{
    public class ResponsavelCreateDTO
    {
        public int IdResponsavel { get; set; }
        public int IdPaciente { get; set; }
    }

    public class ResponsavelUpdateDTO
    {
        public int IdResponsavel { get; set; }
        public int IdPaciente { get; set; }
        public required string Status { get; set; }

    }

}