using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FEL_JAMIRA_WEB_API.Models
{
    [Table("SOLICITACAO")]
    public class Solicitacao
    {
        [Key]
        public int Id { get; set; }
        public DateTime? DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }
        [Required]
        public Double ValorTotal { get; set; }
        [Required]
        public Double ValorTotalEstacionamento { get; set; }
        [Required]
        public Double ValorGanho { get; set; }
        public string TempoEstacionamento { get; set; }
        //0: Em Processo; 1: Fechado; 2: Solicitação Entrar, 3 Solicitação Sair, 4 Recusado.
        public int Status { get; set; }
        [Required]
        public int IdCliente { get; set; }
        [Required]
        public int IdEstacionamento { get; set; }

        [ForeignKey("IdCliente")]
        public Cliente Cliente { get; set; }

        [ForeignKey("IdEstacionamento")]
        public virtual Estacionamento Estacionamento { get; set; }
    }
}
