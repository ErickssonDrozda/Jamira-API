using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FEL_JAMIRA_API.Models.Autenticação.Requisição
{
    public class LoginRequisicao
    {
        public LoginRequisicao(string Login, string Senha)
        {
            this.Login = Login;
            this.Senha = Senha;
        }
        public string Login { get; set; }
        public string Senha { get; set; }
    }
}