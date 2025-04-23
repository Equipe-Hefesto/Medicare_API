using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Medicare_API.Models
{
    public class Remedio
    {
        public int IdRemedio { get; set; }
        public string Nome { get; set; } = null!;
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }

        public List<Promocao> Promocoes { get; set; } = new();
    }

}