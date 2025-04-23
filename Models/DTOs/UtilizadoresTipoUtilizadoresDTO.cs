using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models.DTOs
{
    public class UtilizadorTipoUtilizadorCreateDTO
    {
        public int IdUtilizador { get; set; }
        public int IdTipoUtilizador { get; set; }
    }

    public class UtilizadorTipoUtilizadorUpdateDTO
    {
        public int IdUtilizadorTipoUtilizador { get; set; }
        public int IdUtilizador { get; set; }
        public int IdTipoUtilizador { get; set; }
    }

}