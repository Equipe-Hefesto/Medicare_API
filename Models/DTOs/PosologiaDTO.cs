using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models.DTOs
{
    public class PosologiaDTO
    {
        public required int IdRemedio { get; set; }

        public required int QtdeDose { get; set; }
        public required int IdTipoFarmaceutico { get; set; }
        public required int QtdeConcentracao { get; set; }
        public required int IdTipoGrandeza { get; set; }


        public required int IdTipoAgendamento { get; set; }
        public List<string> Horarios { get; set; } = new();
        public required DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string? Intervalo { get; set; }
        public List<DayOfWeek> DiasSemana { get; set; } = new();
        public int DiasUso { get; set; }
        public int DiasPausa { get; set; }
    }

}