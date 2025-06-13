using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Medicare_API.Models
{
    public class Posologia
    {
        public int IdPosologia { get; set; }
        public int IdUtilizador { get; set; }
        
        public int IdRemedio { get; set; }
        public int QtdeDose { get; set; } = 1;
        public int IdTipoFarmaceutico { get; set; } = 1;
        public int QtdeConcentracao { get; set; } = 1;
        public int IdTipoGrandeza { get; set; } = 1;
        public string? Observacao { get; set; }

        public int IdTipoAgendamento { get; set; }

        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }

        public string? Intervalo { get; set; }
        public List<DayOfWeek> DiasSemana { get; set; } = new();
        public int DiasUso { get; set; }
        public int DiasPausa { get; set; }



        [JsonIgnore]
        public List<Horario> Horarios { get; set; } = new();

        [JsonIgnore]
        public List<Alarme> Alarmes { get; set; } = new();

        [NotMapped]
        public Remedio Remedio { get; set; } = null!;

        [NotMapped]
        public Utilizador Utilizador { get; set; } = null!;

        [NotMapped]
        public TipoFarmaceutico TipoFarmaceutico { get; set; } = null!;

        [NotMapped]
        public TipoGrandeza TipoGrandeza { get; set; } = null!;

        [NotMapped]
        public TipoAgendamento TipoAgendamento { get; set; } = null!;
    }
}