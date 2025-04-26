using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models.DTOs
{
    public class RemedioCreateDTO
    {
        public required string Nome { get; set; }
    }

    public class RemedioUpdateDTO
    {
        public int IdRemedio { get; set; }
        public required string Nome { get; set; }
    }

}