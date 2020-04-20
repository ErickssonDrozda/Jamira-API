using FEL_JAMIRA_WEB_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FEL_JAMIRA_API.Models.MultiModelacao
{
    public class DadosGeraisCliente
    {
        public Usuario usuario { get; set; }
        public Cliente cliente { get; set; }
        public Endereco endereco { get; set; }
    }
}