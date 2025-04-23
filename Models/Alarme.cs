using System;
using System.ComponentModel.DataAnnotations;

namespace Medicare_API.Models
{
    public class Alarme
    {
        public int IdAlarme { get; set; }
        public int IdPosologia { get; set; }
        public DateTime DataHora { get; set; }
        public string Descricao { get; set; } = null!;
        public string Status { get; set; } = null!;

        public Posologia? Posologia { get; set; }
    }

}