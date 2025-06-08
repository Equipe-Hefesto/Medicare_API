using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models.DTOs
{
    public class PosologiaCreateDTO
    {
        public required int IdRemedio { get; set; }
        public required int IdUtilizador { get; set; }

        public required int Quantidade { get; set; }
        public required int IdTipoFarmaceutico { get; set; }
        public required int QuantidadeDose { get; set; }
        public required int IdTipoGrandeza { get; set; }


        public required int IdTipoAgendamento { get; set; }
        public List<string> Horarios { get; set; } = new();
        public required DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string? Intervalo { get; set; }
        public List<string> DiasSemana { get; set; } = new();
        public int DiasUso { get; set; }
        public int DiasPausa { get; set; }
    }

    public class PosologiaUpdateDTO
    {
        public required int IdPosologia { get; set; }
        public required int IdRemedio { get; set; }
        public required int IdUtilizador { get; set; }

        public required int Quantidade { get; set; }
        public required int IdTipoFarmaceutico { get; set; }
        public required int QuantidadeDose { get; set; }
        public required int IdTipoGrandeza { get; set; }
        public List<string> Horarios { get; set; } = new();


        public required int IdTipoAgendamento { get; set; }
        public required DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string? Intervalo { get; set; }
        public List<string> DiasSemana { get; set; } = new();
        public int DiasUso { get; set; }
        public int DiasPausa { get; set; }
    }

    public class PosologiaCompletaDTO
    {
        public Posologia? Posologia { get; set; }
        public string? NomeRemedio { get; set; }
        public Alarme? Alarme { get; set; }

    }
}