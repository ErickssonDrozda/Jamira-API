using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FEL_JAMIRA_WEB_API.Models
{
    [Table("CLIENTE")]
    public class Cliente : Pessoa
    {
        [Key]
        public new int Id { get; set; }
        [Required]
        public string Nickname { get; set; }
        public bool Deletado { get; set; }
        public double Saldo { get; set; }
        public bool TemCarro { get; set; }
        public ICollection<Carro> Carros { get; set; }
    }
}
