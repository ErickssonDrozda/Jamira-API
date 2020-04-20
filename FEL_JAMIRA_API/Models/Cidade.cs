using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FEL_JAMIRA_WEB_API.Models
{
    [Table("CIDADE")]
    public class Cidade
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string NomeCidade { get; set; }
    }
}
