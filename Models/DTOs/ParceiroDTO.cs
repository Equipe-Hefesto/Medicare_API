using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models.DTOs
{
    public class ParceiroCreateDTO
    {
        public required string Nome { get; set; }
        public required string CNPJ { get; set; }
        public required string Apelido { get; set; }
        public required string Status { get; set; }
    }

    public class ParceiroUpdateDTO
    {
        public int IdParceiro { get; set; }
        public required string Nome { get; set; }
        public required string CNPJ { get; set; }
        public required string Apelido { get; set; }
        public required string Status { get; set; }
    }

}