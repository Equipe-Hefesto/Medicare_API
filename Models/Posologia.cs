using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Medicare_API.Models
{
    public class Posologia
    {
        public int IdPosologia { get; set; }
        public int IdRemedio { get; set; }
        public int IdUtilizador { get; set; }
        public int IdTipoFarmaceutico { get; set; }
        public int IdTipoGrandeza { get; set; }
        public int IdTipoAgendamento { get; set; }

        public int Quantidade { get; set; }
        public int QuantidadeDose { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Intervalo { get; set; } = null!;
        public List<string> DiasSemana { get; set; } = new();
        public int DiasUso { get; set; }
        public int DiasPausa { get; set; }

        public Remedio Remedio { get; set; } = null!;
        public Utilizador Utilizador { get; set; } = null!;
        public TipoFarmaceutico TipoFarmaceutico { get; set; } = null!;
        public TipoGrandeza TipoGrandeza { get; set; } = null!;
        public TipoAgendamento TipoAgendamento { get; set; } = null!;
        [JsonIgnore]
        public List<Horario> Horarios { get; set; } = new();
        public List<Alarme> Alarmes { get; set; } = new();

    }
}