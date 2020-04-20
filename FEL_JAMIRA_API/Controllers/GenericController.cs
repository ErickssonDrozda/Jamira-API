using FEL_JAMIRA_API.Models.Cadastros;
using FEL_JAMIRA_WEB_API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace FEL_JAMIRA_API.Controllers
{
    public abstract class GenericController<TEntidade> : ApiController
    where TEntidade : class
    {
        protected Context db = new Context();

        // GET: Generic
        /// <summary>
        /// Método para buscar todos os registros da entidade.
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("~/api/{Controller}/GetAll")]
        public async Task<ResponseViewModel<List<TEntidade>>> GetAll()
        {
            try
            {
                var response = new ResponseViewModel<List<TEntidade>>
                {
                    Data = await db.Set<TEntidade>().AsQueryable().ToListAsync(),
                    Sucesso = true,
                    Mensagem = "Dados retornados com sucesso."
                };
                return response;
            }
            catch (Exception e)
            {
                return new ResponseViewModel<List<TEntidade>>() {
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
        [System.Web.Http.Route("~/api/{Controller}/Detalhes/{id}")]
        public virtual async Task<ResponseViewModel<TEntidade>> Detalhes(int? id)
        {
            try
            {
                if (id == null)
                {
                    var responseError = new ResponseViewModel<TEntidade>
                    {
                        Data = null,
                        Sucesso = false,
                        Mensagem = "Id Nulo"
                    };
                    return responseError;
                }
                TEntidade entidade = await db.Set<TEntidade>().FindAsync(id);
                if (entidade == null)
                {
                    var responseError = new ResponseViewModel<TEntidade>
                    {
                        Data = null,
                        Sucesso = false,
                        Mensagem = "Entidade não encontrada"
                    };
                    return responseError;
                }

                var response = new ResponseViewModel<TEntidade>
                {
                    Data = entidade,
                    Sucesso = true,
                    Mensagem = "Dados carregados com sucesso!"
                };
                return response;
            }
            catch (Exception e)
            {
                return new ResponseViewModel<TEntidade>()
                {
                    Data = null,
                    Serializado = true,
                    Sucesso = false,
                    Mensagem = "Não foi possivel atender a sua solicitação: " + e.Message
                };
            }
        }

        // POST: Generic/Cadastrar
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Método para cadastrar um novo registro.
        /// </summary>
        /// <param name="entidade"></param>
        /// <returns></returns>
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("~/api/{Controller}/Cadastrar")]
        //[ValidateAntiForgeryToken]
        public virtual async Task<ResponseViewModel<TEntidade>> Cadastrar(TEntidade entidade)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Set<TEntidade>().Add(entidade);

                    Task.Run(async () => {
                        await db.SaveChangesAsync();
                    }).Wait();

                    var response = new ResponseViewModel<TEntidade>
                    {
                        Data = entidade,
                        Sucesso = true,
                        Mensagem = "Entidade cadastrada com sucesso!"
                    };
                    return response;
                }
                else
                {
                    var response = new ResponseViewModel<TEntidade>
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
                return new ResponseViewModel<TEntidade>()
                {
                    Data = null,
                    Serializado = true,
                    Sucesso = false,
                    Mensagem = "Não foi possivel atender a sua solicitação: " + e.Message
                };
            }
        }

        // POST: Generic/Editar/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Método para realizar edição do registro passado como parâmetro.
        /// </summary>
        /// <param name="entidade"></param>
        /// <returns></returns>
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPost]
        //[ValidateAntiForgeryToken]
        [System.Web.Http.Route("~/api/{Controller}/Editar")]
        public virtual async Task<ResponseViewModel<TEntidade>> Editar(TEntidade entidade)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry<TEntidade>(entidade).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    var response = new ResponseViewModel<TEntidade>
                    {
                        Data = entidade,
                        Sucesso = true,
                        Mensagem = "Entidade alterada com sucesso."
                    };
                    return response;
                }
                else
                {
                    var response = new ResponseViewModel<TEntidade>
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
                return new ResponseViewModel<TEntidade>()
                {
                    Data = null,
                    Serializado = true,
                    Sucesso = false,
                    Mensagem = "Não foi possivel atender a sua solicitação: " + e.Message
                };
            }
        }

        // POST: Generic/Deletar/5
        /// <summary>
        /// Método para remover um registro de uma entidade.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/{Controller}/Deletar/{id}")]
        //[ValidateAntiForgeryToken]
        public virtual async Task<ResponseViewModel<TEntidade>> Deletar(int id)
        {
            try
            {
                var entidade = await db.Set<TEntidade>().FindAsync(id);
                if (entidade == null)
                {
                    var response = new ResponseViewModel<TEntidade>
                    {
                        Data = null,
                        Sucesso = false,
                        Mensagem = "Entidade não encontrada."
                    };
                    return response;
                }
                else
                {
                    db.Set<TEntidade>().Remove(entidade);
                    await db.SaveChangesAsync();
                    var response = new ResponseViewModel<TEntidade>
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
                return new ResponseViewModel<TEntidade>()
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