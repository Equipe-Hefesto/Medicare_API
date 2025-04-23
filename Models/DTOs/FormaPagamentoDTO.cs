using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models.DTOs
{
    public class FormaPagamentoCreateDTO
    {
        public required string Nome { get; set; }
        public required string Descricao { get; set; }
        public required int QtdeParcelas { get; set; }
        public required int QtdeMinParcelas { get; set; }
        public required int QtdeMaxParcelas { get; set; }
    }


    public class FormaPagamentoUpdateDTO
    {
        public required int IdFormaPagamento { get; set; }
        public required string Nome { get; set; }
        public required string Descricao { get; set; }
        
        public required int QtdeParcelas { get; set; }
        public required int QtdeMinParcelas { get; set; }
        public required int QtdeMaxParcelas { get; set; }
    }

}