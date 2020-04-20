using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FEL_JAMIRA_WEB_API.Models
{
    [Table("ESTADO")]
    public class Estado
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Sigla { get; set; }
        [Required]
        public string NomeEstado { get; set; }
    }
}
