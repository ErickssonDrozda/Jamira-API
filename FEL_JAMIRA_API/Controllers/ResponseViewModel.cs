using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FEL_JAMIRA_API.Controllers
{
    public class ResponseViewModel<T>
    {
        public T Data { get; set; }
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public bool Serializado { get; set; } = true;
    }
}