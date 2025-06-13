using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medicare_API.Models
{
    public class Alarme
    {
        public int IdAlarme { get; set; }
        public int IdPosologia { get; set; }
        public DateTime DataHora { get; set; }
        public string Status { get; set; } = "P";
        public int ContadorSoneca { get; set; } = 0;

        [NotMapped]
        public Posologia? Posologia { get; set; }
    }

}