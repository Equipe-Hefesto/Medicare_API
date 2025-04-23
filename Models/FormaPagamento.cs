using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Medicare_API.Models
{
    public class FormaPagamento
    {
        public int IdFormaPagamento { get; set; }
        public string Descricao { get; set; } = null!;
        public int QtdeParcelas { get; set; }
        public int QtdeMinParcelas { get; set; }
        public int QtdeMaxParcelas { get; set; }
    }

}
