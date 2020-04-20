using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FEL_JAMIRA_WEB_API.Models
{
    [Table("FALE_CONOSCO")]
    public class FaleConosco
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Texto { get; set; }
        public DateTime DataCriacao { get; set; }
        [Required]
        public int IdPessoa { get; set; }
        public int IdCategoria { get; set; }

        [ForeignKey("IdCategoria")]
        public virtual CategoriaFaleConosco CategoriaFaleConosco { get; set; }
        [ForeignKey("IdPessoa")]
        public virtual Pessoa Pessoa { get; set; }
    }
}
