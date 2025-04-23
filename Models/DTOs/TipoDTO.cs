using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models.DTOs
{
    public class TipoCreateDTO
    {

        public required string Descricao { get; set; }
    }
    public class TipoUpdateDTO
    {
        public required int IdTipo { get; set; }
        public required string Descricao { get; set; }
    }

}