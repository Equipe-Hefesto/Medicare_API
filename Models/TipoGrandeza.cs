using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Medicare_API.Models
{
    public class TipoGrandeza
    {
        public int IdTipoGrandeza { get; set; }
        public string Descricao { get; set; } = null!;
    }
}