using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Medicare_API.Models
{
    public class TipoUtilizador
    {
        public int IdTipoUtilizador { get; set; }
        public string Descricao { get; set; } = null!;

        [JsonIgnore]
        public List<UtilizadorTipoUtilizador> Utilizadores { get; set; } = new();
    }

}