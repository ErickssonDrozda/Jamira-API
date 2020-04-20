using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FEL_JAMIRA_WEB_API.Models
{
    [Table("USUARIO")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Senha { get; set; }
        [Required]
        public string Nome { get; set; }
        public int Level { get; set; }
        public int IdPessoa { get; set; }
        public Byte[] Foto { get; set; } 


        [ForeignKey("IdPessoa")]
        public Pessoa Pessoa { get; set; }
        public string AuxSenha { get; set; }
    }
}
