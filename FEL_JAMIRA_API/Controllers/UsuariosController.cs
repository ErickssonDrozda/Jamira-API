using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using FEL_JAMIRA_WEB_API.Models;
using FEL_JAMIRA_API.Models.Autenticação.Requisição;
using FEL_JAMIRA_API.Util;
using System.Web.Http;

namespace FEL_JAMIRA_API.Controllers
{
    public class UsuariosController : ApiController
    {
        protected Context db = new Context();
        [HttpPost]
        public async Task<ResponseViewModel<Usuario>> Login(LoginRequisicao loginRequisicao)
        {
            try
            {
                if (!string.IsNullOrEmpty(loginRequisicao.Login) && !string.IsNullOrEmpty(loginRequisicao.Senha))
                {
                    Usuario usuario = new Usuario();
                    Task.Run(async () => {
                        Usuario usuarioRetorno = await db.Usuarios.FirstOrDefaultAsync(x => x.Login.Equals(loginRequisicao.Login));
                        usuario = usuarioRetorno;
                    }).Wait();
                    if (usuario == null || string.IsNullOrEmpty(usuario.AuxSenha))
                    {
                        return new ResponseViewModel<Usuario>()
                        {
                            Data = null,
                            Serializado = true,
                            Sucesso = false,
                            Mensagem = "Invalid Login"
                        };
                    }
                    string senha = Helpers.CriarSenha(loginRequisicao.Senha, usuario.AuxSenha);
                    Task.Run(async () => {
                        Usuario usuarioRetorno = await db.Usuarios.Include("Pessoa").FirstOrDefaultAsync(
                            x => x.Login.Equals(loginRequisicao.Login) &&
                            x.Senha.Equals(senha));
                        usuario = usuarioRetorno;
                    }).Wait();
                    if (usuario != null)
                    {
                        return new ResponseViewModel<Usuario>()
                        {
                            Data = usuario,
                            Serializado = true,
                            Sucesso = true,
                            Mensagem = "Dados retornados com sucesso."
                        };
                    }
                        else {
                        return new ResponseViewModel<Usuario>()
                        {
                            Data = null,
                            Serializado = true,
                            Sucesso = false,
                            Mensagem = "Login ou Senha não foram definidos, por favor insira-os."
                        };
                    }
                }
                else
                {
                    return new ResponseViewModel<Usuario>()
                    {
                        Data = null,
                        Serializado = true,
                        Sucesso = false,
                        Mensagem = "Login ou Senha não foram definidos, por favor insira-os."
                    };
                }
            }
            catch (Exception e)
            {
                return new ResponseViewModel<Usuario>()
                {
                    Data = null,
                    Serializado = true,
                    Sucesso = false,
                    Mensagem = "Não foi possivel atender a sua solicitação: " + e.Message
                };
            }
        }


        /// <summary>
        /// Método para buscar um registro em especifico.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Generic/Detalhes/5
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("~/api/Usuarios/Detalhes/{id}")]
        public virtual async Task<ResponseViewModel<Usuario>> Detalhes(int? id)
        {
            try
            {
                if (id == null)
                {
                    var responseError = new ResponseViewModel<Usuario>
                    {
                        Data = null,
                        Sucesso = false,
                        Mensagem = "Id Nulo"
                    };
                    return responseError;
                }
                Usuario entidade = await db.Usuarios.FindAsync(id);
                if (entidade == null)
                {
                    var responseError = new ResponseViewModel<Usuario>
                    {
                        Data = null,
                        Sucesso = false,
                        Mensagem = "Entidade não encontrada"
                    };
                    return responseError;
                }

                var response = new ResponseViewModel<Usuario>
                {
                    Data = entidade,
                    Sucesso = true,
                    Mensagem = "Dados carregados com sucesso!"
                };
                return response;
            }
            catch (Exception e)
            {
                return new ResponseViewModel<Usuario>()
                {
                    Data = null,
                    Serializado = true,
                    Sucesso = false,
                    Mensagem = "Não foi possivel atender a sua solicitação: " + e.Message
                };
            }
        }
    }
}
