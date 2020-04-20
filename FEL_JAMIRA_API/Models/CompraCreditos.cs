using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FEL_JAMIRA_WEB_API.Models
{
    [Table("COMPRA_CREDITOS")]
    public class CompraCreditos
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int IdCliente { get; set; }
        [Required]
        public DateTime DataTransacao { get; set; }
        public double Credito { get; set; }
        [ForeignKey("IdCliente")]
        public virtual Cliente Cliente { get; set; }
        
    }
}
