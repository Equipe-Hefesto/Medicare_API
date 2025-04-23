using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Medicare_API.Models
{
    public class TipoParentesco
    {
        public int IdTipoParentesco { get; set; }
        public string Descricao { get; set; } = null!;
    }

}