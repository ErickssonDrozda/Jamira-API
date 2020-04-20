using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FEL_JAMIRA_API.Models
{
    [Table("BANCO")]
    public class Banco
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}