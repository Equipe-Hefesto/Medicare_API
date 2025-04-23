using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models
{
    public class TipoFarmaceutico
    {
        public int IdTipoFarmaceutico { get; set; }
        public string Descricao { get; set; } = null!;
    }

}