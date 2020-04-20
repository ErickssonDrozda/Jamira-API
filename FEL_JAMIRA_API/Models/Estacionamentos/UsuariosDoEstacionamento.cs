using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FEL_JAMIRA_API.Models.Estacionamentos
{
    public class UsuariosDoEstacionamento
    {
        public string NomeUsuario { get; set; }
        public DateTime? PeriodoDe { get; set; }
        public DateTime? PeriodoAte { get; set; }
        public double ValorGanho { get; set; }

    }
}