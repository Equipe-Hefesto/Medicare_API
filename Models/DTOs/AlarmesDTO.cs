using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models.DTOs
{
    public class AlarmeDTO
    {
        public required int IdPosologia { get; set; }
        public required DateTime DataHora { get; set; }
        public required string Status { get; set; } = null!;
    }

    public class AlarmeAgendarDTO
    {
        public required Alarme Alarme { get; set; }
        public required string NomeRemedio { get; set; }
        public required string Dose { get; set; }
        public required string Observacao { get; set; }
        public required Soneca Soneca { get; set; }

    }
    public class AlarmeCompletoDTO : AlarmeAgendarDTO
    {
        public required string Frequencia { get; set; }
        public required string Concentracao { get; set; }
        public required string Proximo { get; set; }
    }

    public class AlarmeStatusDTO
    {
        public required string Status { get; set; }
    }

    public class AlarmeContadorDTO
    {
        public required DateTime DataHora { get; set; }
        public int ContadorSoneca { get; set; }
    }
}