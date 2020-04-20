using FEL_JAMIRA_WEB_API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FEL_JAMIRA_API.Models
{
    [Table("CONTA_DEPOSITO")]
    public class ContaDeposito
    {
        [Key]
        public int Id { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public int IdEstacionamento { get; set; }
        public int IdBanco { get; set; }
        public int IdTipoConta { get; set; }
        [ForeignKey("IdBanco")]
        public virtual Banco Banco { get; set; }
        [ForeignKey("IdTipoConta")]
        public virtual TipoConta TipoConta { get; set; }
        [ForeignKey("IdEstacionamento")]
        public virtual Estacionamento Estacionamento { get; set; }
    }
}