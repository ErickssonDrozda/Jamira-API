using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FEL_JAMIRA_WEB_API.Models
{
    [Table("ENDERECO")]
    public class Endereco
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Rua { get; set; }
        [Required]
        public int Numero { get; set; }
        [Required]
        public string Bairro { get; set; }
        public string CEP { get; set; }
        public string Complemento { get; set; }
        [Required]
        public int IdCidade { get; set; }
        [Required]
        public int IdEstado { get; set; }

        [ForeignKey("IdCidade")]
        public virtual Cidade Cidade { get; set; }

        [ForeignKey("IdEstado")]
        public virtual Estado Estado { get; set; }
    }
}
