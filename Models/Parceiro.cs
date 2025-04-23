using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Medicare_API.Models
{
    public class Parceiro
    {
        public int IdParceiro { get; set; }
        public string Nome { get; set; } = null!;
        public string Apelido { get; set; } = null!;
        public string CNPJ { get; set; } = null!;
        public string Status { get; set; } = null!;

        public List<ParceiroUtilizador> ParceirosUtilizadores { get; set; } = new();
    }
}