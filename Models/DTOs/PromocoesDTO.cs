using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models.DTOs
{
    public class PromocaoCreateDTO
    {
        public int IdFormaPagamento { get; set; }
        public int IdUtilizador { get; set; }
        public int IdRemedio { get; set; }

        public string Descricao { get; set; } = null!;
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public decimal Valor { get; set; }
    }

    public class PromocaoUpdateDTO
    {
        public int IdPromocao { get; set; }
        public int IdFormaPagamento { get; set; }
        public int IdUtilizador { get; set; }
        public int IdRemedio { get; set; }

        public string Descricao { get; set; } = null!;
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public decimal Valor { get; set; }
    }

}