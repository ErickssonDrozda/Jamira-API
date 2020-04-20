using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FEL_JAMIRA_API.Models.Administração
{
    public class RetornoFaleConosco
    {
        public string EmailUsuario { get; set; }
        public int? IdPessoa { get; set; }
        public string Mensagem{ get; set; }
        public string Categoria { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}