using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models.DTOs
{
    public class UtilizadorCreateDTO
    {
        public required string CPF { get; set; }
        public required string Nome { get; set; }
        public required string Sobrenome { get; set; }
        public required DateTime DtNascimento { get; set; }
        public required string Email { get; set; }
        public required string Telefone { get; set; }
        public required string SenhaString { get; set; }
    }

    public class UtilizadorUpdateDTO
    {
        //public int IdUtilizador { get; set; }
        public required string CPF { get; set; }
        public required string Nome { get; set; }
        public required string Sobrenome { get; set; }
        public required DateTime DtNascimento { get; set; }
        public required string Email { get; set; }
        public required string Telefone { get; set; }
        public required string SenhaString { get; set; }
    }

    public class UtilizadorAutenticarDTO
    {
        public required string Nome { get; set; }
        public required string SenhaString { get; set; }
    }
}