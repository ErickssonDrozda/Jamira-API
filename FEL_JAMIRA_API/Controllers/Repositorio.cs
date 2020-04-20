using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace FEL_JAMIRA_API.Controllers
{
    interface Repositorio<T> where T : class
    {
        Task<JsonResult> GetTodos();
        //IQueryable<T> GetTodos();
        T Procurar(params object[] key);
        void Adicionar(T entity);
        void Atualizar(T entity);
        void Deletar(Func<T, bool> predicate);
        void Commit();
        void Dispose();
    }
}