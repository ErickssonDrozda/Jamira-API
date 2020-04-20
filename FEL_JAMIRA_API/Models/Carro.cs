using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FEL_JAMIRA_WEB_API.Models
{
    [Table("CARROS")]
    public class Carro
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Placa { get; set; }
        [Required]
        public string Porte { get; set; }
        [Required]
        public string Modelo { get; set; }
        public int IdMarca { get; set; }
        public int IdCliente { get; set; }
        [ForeignKey("IdMarca")]
        public virtual Marca Marca { get; set; }
        [ForeignKey("IdCliente")]
        public virtual Cliente Cliente { get; set; }
    }
}
