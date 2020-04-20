using FEL_JAMIRA_API.Controllers;
using FEL_JAMIRA_API.Models.Autenticação.Requisição;
using FEL_JAMIRA_WEB_API.Models;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace FEL_JAMIRA_API.Token
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            UsuariosController usuariosController = new UsuariosController();
            ResponseViewModel<Usuario> responseViewModel = new ResponseViewModel<Usuario>();
            Task.Run(async () =>
            {
                ResponseViewModel<Usuario> verificaAcesso = await usuariosController.Login(new LoginRequisicao(context.UserName, context.Password));
                responseViewModel = verificaAcesso;
            }).Wait();
            //if (FuncionariosSeguranca.Login(context.UserName, context.Password))
            if (responseViewModel.Sucesso.Equals(true))
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("sub", context.UserName));
                identity.AddClaim(new Claim("role", "user"));

                context.Validated(identity);
            }
            else
            {
                context.SetError("acesso inválido", "As credenciais do usuário não conferem....");
                return;
            }
        }

    }
}