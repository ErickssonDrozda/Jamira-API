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
using System.Net.Mail;
using FEL_JAMIRA_API.Models.Administração;

namespace FEL_JAMIRA_API.Controllers
{
    public class FaleConoscoController : GenericController<FaleConosco>
    {
        [Authorize]
        public async Task<ResponseViewModel<bool>> EnviarEmail(string email, string titulo, string corpo)
        {
            try
            {
                //Conta de email para fazer o envio...
                string Conta = "fel.jamira.brasil@gmail.com";
                string Senha = "j@mira123";

                //Montar o email...
                MailMessage msg = new MailMessage(Conta, email);
                //de->para
                msg.Subject = titulo; //assunto da mensagem
                msg.IsBodyHtml = false;
                msg.Body = corpo; //corpo da mensagem

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

                return new ResponseViewModel<bool>
                {
                    Data = true,
                    Mensagem = "Message sent successfully!",
                    Serializado = true,
                    Sucesso = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseViewModel<bool>
                {
                    Data = true,
                    Mensagem = "The message could not be sent. " + ex.Message,
                    Serializado = true,
                    Sucesso = true
                };
            }
        }

        [HttpPost]
        public async Task<ResponseViewModel<FaleConosco>> FaleComigo(MensagemEnvio mensagem)  
        {
            try
            {
                //Conta de email para fazer o envio...
                string Conta = "fel.jamira.brasil@gmail.com";
                string Senha = "j@mira123";

                //Montar o email...
                MailMessage msg = new MailMessage(Conta, "erickssondrozda@outlook.com");
                //de->para
                msg.Subject = "Institucional Jamira"; //assunto da mensagem
                msg.IsBodyHtml = false;
                msg.Body = mensagem.mensagem; //corpo da mensagem

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

                return new ResponseViewModel<FaleConosco>
                {
                    Data = null,
                    Mensagem = "Message sent successfully!",
                    Serializado = true,
                    Sucesso = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseViewModel<FaleConosco>
                {
                    Data = null,
                    Mensagem = "The message could not be sent. " + ex.Message,
                    Serializado = true,
                    Sucesso = false
                };
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ResponseViewModel<RetornoFaleConosco>> GetFaleConoscoUsuario(int IdFaleConosco)
        {
            try
            {
                FaleConosco faleConosco = db.FaleConosco.Include("CategoriaFaleConosco").FirstOrDefault(x => x.Id.Equals(IdFaleConosco));
                Usuario usuario = db.Usuarios.FirstOrDefault(x => x.IdPessoa.Equals(faleConosco.IdPessoa));

                RetornoFaleConosco retorno = new RetornoFaleConosco
                { 
                    Categoria = faleConosco.CategoriaFaleConosco.NomeCategoria,
                    DataCriacao = faleConosco.DataCriacao,
                    IdPessoa = faleConosco.IdPessoa,
                    EmailUsuario = usuario.Login,
                    Mensagem = faleConosco.Texto
                };

                return new ResponseViewModel<RetornoFaleConosco>
                {
                    Data = retorno,
                    Mensagem = "Dados retornados com sucesso!",
                    Serializado = true,
                    Sucesso = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseViewModel<RetornoFaleConosco>
                {
                    Data = null,
                    Mensagem = "Não foi possivel enviar a mensagem. " + ex.Message,
                    Serializado = true,
                    Sucesso = true
                };
            }
        }
    }
}
