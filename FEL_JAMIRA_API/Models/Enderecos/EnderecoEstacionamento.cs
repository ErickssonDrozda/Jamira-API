using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FEL_JAMIRA_API.Models.Enderecos
{
    public class EnderecoEstacionamento
    {
        public string Rua { get; set; }
        public int Numero { get; set; }
        public string Bairro { get; set; }
        public string CEP { get; set; }
        public string Complemento { get; set; }
        public int IdCidade { get; set; }
        public int IdEstado { get; set; }
        public int IdPessoa { get; set; }
    }
}