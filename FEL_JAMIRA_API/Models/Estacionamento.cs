using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FEL_JAMIRA_WEB_API.Models
{
    [Table("ESTACIONAMENTO")]
    public class Estacionamento
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string NomeEstacionamento { get; set; }
        [Required]
        public string CNPJ { get; set; }
        [Required]
        public int IdPessoa{ get; set; }
        public string InscricaoEstadual { get; set; }
        public bool TemEstacionamento { get; set; }
        public bool Deletado { get; set; }
        public int ValorHora { get; set; }
        public int? IdEnderecoEstabelecimento { get; set; }

        [ForeignKey("IdEnderecoEstabelecimento")]
        public virtual Endereco EnderecoEstacionamento { get; set; }
        [ForeignKey("IdPessoa")]
        public virtual Pessoa Proprietario { get; set; }
    }
}
