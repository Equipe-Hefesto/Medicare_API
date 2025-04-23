using System;
using System.ComponentModel.DataAnnotations;


namespace Medicare_API.Models
{
    public class Promocao
    {
        public int IdPromocao { get; set; }
        public int IdFormaPagamento { get; set; }
        public int IdUtilizador { get; set; }
        public int IdRemedio { get; set; }

        public string Descricao { get; set; } = null!;
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public decimal Valor { get; set; }

        public FormaPagamento? FormaPagamento { get; set; }
        public Utilizador? Utilizador { get; set; }
        public Remedio? Remedio { get; set; }
    }

}
