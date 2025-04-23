using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medicare_API.Models
{
    public class Responsavel
    {
        public int IdResponsavel { get; set; }
        public int IdPaciente { get; set; }
        public int IdTipoParentesco { get; set; }

        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string Status { get; set; } = null!;

        public Utilizador? ResponsavelUser { get; set; }
        public Utilizador? Paciente { get; set; }
        public TipoParentesco? TipoParentesco { get; set; }
    }


}
