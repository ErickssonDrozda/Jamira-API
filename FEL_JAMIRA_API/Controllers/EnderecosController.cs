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
using FEL_JAMIRA_API.Models.Enderecos;

namespace FEL_JAMIRA_API.Controllers
{
    public class EnderecosController : GenericController<Endereco>
    {
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("~/api/Enderecos/Registrar")]
        //[ValidateAntiForgeryToken]
        public virtual async Task<ResponseViewModel<EnderecoEstacionamento>> RegistrarEndereco(EnderecoEstacionamento enderecoEstacionamento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (EstacionamentosController estacionamentosController = new EstacionamentosController())
                    {
                        Estacionamento entidade =
                         db.Estacionamentos.Include("Proprietario").Where(x => x.IdPessoa.Equals(enderecoEstacionamento.IdPessoa)).FirstOrDefault();
                        entidade.EnderecoEstacionamento = new Endereco
                        {
                            Rua = enderecoEstacionamento.Rua,
                            Numero = enderecoEstacionamento.Numero,
                            Bairro = enderecoEstacionamento.Bairro,
                            CEP = enderecoEstacionamento.CEP,
                            Complemento = enderecoEstacionamento.Complemento,
                            IdCidade = enderecoEstacionamento.IdCidade,
                            IdEstado = enderecoEstacionamento.IdEstado
                        };
                        entidade.TemEstacionamento = true;
                        db.Entry(entidade).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return new ResponseViewModel<EnderecoEstacionamento>
                        {
                            Data = enderecoEstacionamento,
                            Sucesso = true,
                            Mensagem = "Endereço cadastrado com sucesso."
                        };

                    };
                }
                else
                {
                    var response = new ResponseViewModel<EnderecoEstacionamento>
                    {
                        Data = enderecoEstacionamento,
                        Sucesso = false,
                        Mensagem = "Erro na validação da entidade."
                    };
                    return response;
                }
            }
            catch (Exception e)
            {
                return new ResponseViewModel<EnderecoEstacionamento>()
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
