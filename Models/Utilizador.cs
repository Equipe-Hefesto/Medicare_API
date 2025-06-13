using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Medicare_API.Data;
using Microsoft.AspNetCore.Identity;

namespace Medicare_API.Models
{
    public class Utilizador
    {
        public int IdUtilizador { get; set; }
        public string Nome { get; set; } = null!;
        public string Sobrenome { get; set; } = null!;
        public string CPF { get; set; } = null!;
        public DateTime DtNascimento { get; set; }
        public string Email { get; set; } = null!;
        public string Telefone { get; set; } = null!;
        public byte[]? SenhaHash { get; set; } = null!;
        public byte[]? SenhaSalt { get; set; } = null!;
        public string Username { get; set; } = null!;
        [NotMapped]
        public string Token { get; set; } = string.Empty;        public string? PasswordResetToken { get; set; }

        public DateTime? PasswordResetTokenExpiration { get; set; }

        [JsonIgnore]
        public List<UtilizadorTipoUtilizador> TiposUtilizadores { get; set; } = new();
        public List<Responsavel> Responsaveis { get; set; } = new();
        public List<Cuidador> Cuidadores { get; set; } = new();
        public List<ParceiroUtilizador> ParceirosUtilizadores { get; set; } = new();
        public List<Posologia> Posologias { get; set; } = new();
        public List<Promocao> Promocoes { get; set; } = new();
    }

}