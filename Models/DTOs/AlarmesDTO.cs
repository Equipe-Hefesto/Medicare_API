using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models.DTOs
{
    public class AlarmeCreateDTO
    {
        public required int IdPosologia { get; set; }
        public required DateTime DataHora { get; set; }
        public required string Descricao { get; set; } = null!;
        public required string Status { get; set; } = null!;
    }
    public class AlarmeUpdateDTO
    {
        public required int IdAlarme { get; set; }
        public required int IdPosologia { get; set; }
        public required DateTime DataHora { get; set; }
        public required string Descricao { get; set; } = null!;
        public required string Status { get; set; } = null!;
    }
    public class AlarmeComRemedioDTO
    {
        public  Alarme? Alarme { get; set; }
        public string? NomeRemedio { get; set; }

    }
    public class AlarmeStatusUpdateDTO
{
    public required string Status { get; set; }
}
}