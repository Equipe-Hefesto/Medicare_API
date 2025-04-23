using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Medicare_API.Models
{
  public class Cuidador
{
    public int IdCuidador { get; set; }
    public int IdPaciente { get; set; }

    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataAtualizacao { get; set; }
    public string Status { get; set; } = null!;

    public Utilizador? CuidadorUser { get; set; }
    public Utilizador? Paciente { get; set; }
}

}
