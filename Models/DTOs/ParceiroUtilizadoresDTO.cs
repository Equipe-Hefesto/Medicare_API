using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models.DTOs
{
    public class ParceiroUtilizadorCreateDTO
    {
        public int IdUtilizador { get; set; }
        public int IdParceiro { get; set; }
    }

    public class ParceiroUtilizadorUpdateDTO
    {
        public int IdUtilizador { get; set; }
        public int IdParceiro { get; set; }
    }

}