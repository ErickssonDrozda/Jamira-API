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
using FEL_JAMIRA_API.Models.Clientes;
using System.Net.Mail;

namespace FEL_JAMIRA_API.Controllers
{
    public class CompraCreditosController : GenericController<CompraCreditos>
    {
        /// <summary>
        /// Método para Buscar os recebimentos do usuários.
        /// </summary>
        /// <param name="entidade"></param>
        /// <returns></returns>
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("~/api/CompraCreditos/GetTransacoesDoCliente")]
        public async Task<ResponseViewModel<List<TransacoesDoCliente>>> GetTransacoesDoCliente(string idUsuario)
        {
            try
            {
                int valor = int.Parse(idUsuario);
                List<TransacoesDoCliente> solicitacao = await db.CompraCreditos
                                            .Where(x => x.IdCliente.Equals(valor))
                                            .Select(cl => new TransacoesDoCliente
                                            {
                                                DataTransacao = cl.DataTransacao,
                                                Valor = cl.Credito
                                            }).ToListAsync();
                ResponseViewModel<List<TransacoesDoCliente>> response = new ResponseViewModel<List<TransacoesDoCliente>>
                {
                    Data = solicitacao,
                    Mensagem = "Dados retornados com sucesso!",
                    Serializado = true,
                    Sucesso = true
                };

                return response;
            }
            catch (Exception e)
            {
                return new ResponseViewModel<List<TransacoesDoCliente>>()
                {
                    Data = null,
                    Serializado = true,
                    Sucesso = false,
                    Mensagem = "Não foi possivel atender a sua solicitação: " + e.Message
                };
            }
        }

        /// <summary>
        /// Método para Buscar os Creditar conta do cliente.
        /// </summary>
        /// <param name="entidade"></param>
        /// <returns></returns>
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("~/api/CompraCreditos/CreditarConta")]
        public async Task<ResponseViewModel<CompraCreditos>> CreditarConta(CompraCreditos compraCreditos)
        {
            try
            {
                Task.Run(async () => {
                    await Cadastrar(compraCreditos);
                }).Wait();

                Cliente cliente = db.Clientes.Find(compraCreditos.IdCliente);

                cliente.Saldo += compraCreditos.Credito;

                Task.Run(async () => {
                    db.Entry<Cliente>(cliente).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }).Wait();

                EnviarEmailCliente(cliente, compraCreditos.Credito);

                ResponseViewModel<CompraCreditos> response = new ResponseViewModel<CompraCreditos>
                {
                    Data = compraCreditos,
                    Mensagem = "Dados Cadastrados com sucesso!",
                    Serializado = true,
                    Sucesso = true
                };

                return response;
            }
            catch (Exception e)
            {
                return new ResponseViewModel<CompraCreditos>()
                {
                    Data = null,
                    Serializado = true,
                    Sucesso = false,
                    Mensagem = "Não foi possivel atender a sua solicitação: " + e.Message
                };
            }
        }

        public void EnviarEmailCliente(Cliente cliente, double reais)
        {
            try
            {
                Usuario usuario = db.Usuarios.FirstOrDefault(x => x.IdPessoa == ((FEL_JAMIRA_WEB_API.Models.Pessoa)cliente).Id);
                //Conta de email para fazer o envio...
                string Conta = "fel.jamira.brasil@gmail.com";
                string Senha = "j@mira123";

                //Montar o email...
                MailMessage msg = new MailMessage(Conta, usuario.Login);
                //de->para
                msg.Subject = "Confirmação de Pagamento"; //assunto da mensagem
                msg.IsBodyHtml = true;
                msg.Body = "<b>Sucesso!</b><br /><p>Confirmamos a transação de seu pagamento no valor de " + reais + " reais</p><br /><br /><br /> <p>Você já pode estar usufruindo do novo saldo. faça bom aproveito. :) </p><br /><br /><br /><br /> Att, Equipe Jamira"; //corpo da mensagem

                //Enviar o email...
                //SMTP (Simple Mail Transfer Protocol)
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(Conta, Senha);
                smtp.EnableSsl = true; //Security Socket de Layer
                
                //autenticação
                smtp.Send(msg); //enviando a mensagem
            }
            catch (Exception ex)
            {              
            }
        }
    }
}
