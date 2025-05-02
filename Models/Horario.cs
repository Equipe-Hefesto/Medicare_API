using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Medicare_API.Models
{
    public class Horario
    {
        public int IdPosologia { get; set; }
        public TimeOnly Hora { get; set; }
        [JsonIgnore]
        public Posologia? Posologia { get; set; }
    }

}