using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FEL_JAMIRA_API.Models.MultiModelacao
{
    public class DadosCliente
    {
        public string Nome { get; set; }
        public string Nickname { get; set; }
        public string CPF { get; set; }
        public DateTime? Nascimento { get; set; }
        public string RG { get; set; }
        public string Email { get; set; }
        public string Rua { get; set; }
        public int Numero { get; set; }
        public string Bairro { get; set; }
        public string CEP { get; set; }
        public string Complemento { get; set; }
        public int IdCidade { get; set; }
        public int IdEstado { get; set; }
    }
}