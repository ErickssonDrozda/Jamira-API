using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FEL_JAMIRA_API.Models.MultiModelacao
{
    public class DadosEstacionamento
    {
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public int IdBanco { get; set; }
        public int IdTipoConta { get; set; }
        public string NomeProprietario { get; set; }
        public string NomeEstacionamento { get; set; }
        public DateTime? Nascimento { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public int ValorHora { get; set; }
        public string Nickname { get; set; }
        public string CNPJ { get; set; }
        public string InscricaoEstadual { get; set; }
        public string Email { get; set; }
        public string Rua { get; set; }
        public int Numero { get; set; }
        public string Bairro { get; set; }
        public string CEP { get; set; }
        public string Complemento { get; set; }
        public int IdCidade { get; set; }
        public int IdEstado { get; set; }

        //Endereço do Estabelecimento

        public string RuaEstacionamento { get; set; }
        public int? NumeroEstacionamento { get; set; }
        public string BairroEstacionamento { get; set; }
        public string CEPEstacionamento { get; set; }
        public string ComplementoEstacionamento { get; set; }
        public int? IdCidadeEstacionamento { get; set; }
        public int? IdEstadoEstacionamento { get; set; }

    }
}