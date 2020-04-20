using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FEL_JAMIRA_WEB_API.Models
{
    [Table("PESSOA")]
    public class Pessoa
    {
        [Key]
        public int Id { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        [Required]
        public string Nome { get; set; }
        public DateTime? Nascimento { get; set; }
        public DateTime DataCriacao { get; set; }
        public int IdEndereco { get; set; }

        [ForeignKey("IdEndereco")]
        public virtual Endereco EnderecoPessoa { get; set; }
    }
}
