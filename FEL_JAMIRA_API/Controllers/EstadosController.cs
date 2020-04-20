using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FEL_JAMIRA_WEB_API.Models;
using System.Web.Http;

namespace FEL_JAMIRA_API.Controllers
{
    public class EstadosController : ApiController
    {
        protected Context db = new Context();

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("~/api/Estados/GetAll")]
        public async Task<ResponseViewModel<List<Estado>>> GetAll()
        {
            try
            {
                var response = new ResponseViewModel<List<Estado>>
                {
                    Data = await db.Estados.AsQueryable().ToListAsync(),
                    Sucesso = true,
                    Mensagem = "Dados retornados com sucesso."
                };
                return response;
            }
            catch (Exception e)
            {
                return new ResponseViewModel<List<Estado>>()
                {
                    Data = null,
                    Serializado = true,
                    Sucesso = false,
                    Mensagem = "Não foi possivel atender a sua solicitação: " + e.Message
                };
            }

        }

        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("~/api/Estados/Cadastrar")]
        //[ValidateAntiForgeryToken]
        public virtual async Task<ResponseViewModel<Estado>> Cadastrar(Estado entidade)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Estados.Add(entidade);

                    Task.Run(async () => {
                        await db.SaveChangesAsync();
                    }).Wait();

                    var response = new ResponseViewModel<Estado>
                    {
                        Data = entidade,
                        Sucesso = true,
                        Mensagem = "Entidade cadastrada com sucesso!"
                    };
                    return response;
                }
                else
                {
                    var response = new ResponseViewModel<Estado>
                    {
                        Data = entidade,
                        Sucesso = false,
                        Mensagem = "Erro na validação da entidade."
                    };
                    return response;
                }
            }
            catch (Exception e)
            {
                return new ResponseViewModel<Estado>()
                {
                    Data = null,
                    Serializado = true,
                    Sucesso = false,
                    Mensagem = "Não foi possivel atender a sua solicitação: " + e.Message
                };
            }
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Estados/Deletar/{id}")]
        public virtual async Task<ResponseViewModel<Estado>> Deletar(int id)
        {
            try
            {
                var entidade = await db.Estados.FindAsync(id);
                if (entidade == null)
                {
                    var response = new ResponseViewModel<Estado>
                    {
                        Data = null,
                        Sucesso = false,
                        Mensagem = "Entidade não encontrada."
                    };
                    return response;
                }
                else
                {
                    db.Estados.Remove(entidade);
                    await db.SaveChangesAsync();
                    var response = new ResponseViewModel<Estado>
                    {
                        Data = null,
                        Sucesso = true,
                        Mensagem = "Entidade removida com sucesso."
                    };
                    return response;
                }
            }
            catch (Exception e)
            {
                return new ResponseViewModel<Estado>()
                {
                    Data = null,
                    Serializado = true,
                    Sucesso = false,
                    Mensagem = "Não foi possivel atender a sua solicitação: " + e.Message
                };
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
