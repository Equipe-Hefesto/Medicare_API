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
        public required string Username { get; set; }

        public required string SenhaString { get; set; }
    }

    public class UtilizadorUpdateDTO
    {
        public int IdUtilizador { get; set; }
        public required string CPF { get; set; }
        public required string Nome { get; set; }
        public required string Sobrenome { get; set; }
        public required DateTime DtNascimento { get; set; }
        public required string Username { get; set; }

        public required string Email { get; set; }
        public required string Telefone { get; set; }
        public required string SenhaString { get; set; }
    }

    public class UtilizadorAutenticarDTO
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string SenhaString { get; set; }
    }

    public class ValidarCpfDTO
    {
        public required string CPF { get; set; }

    }

    public class ValidarEmailDTO
    {
        public required string Email { get; set; }

    }

    public class ForgotPasswordRequest
    {
        public string Email { get; set; } = string.Empty;
    }

    public class ResetPasswordRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }



}