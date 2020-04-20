using FEL_JAMIRA_WEB_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FEL_JAMIRA_API.Models.MultiModelacao
{
    public class DadosGeraisEstacionamento
    {
        public Endereco enderecoUsuario { get; set; }
        public Estacionamento estacionamento { get; set; }
        public Usuario usuario { get; set; }
        public Pessoa pessoa { get; set; }
        public ContaDeposito contaDeposito { get; set; }

    }
}