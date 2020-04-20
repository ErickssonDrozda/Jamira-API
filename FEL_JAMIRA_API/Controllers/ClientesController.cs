using FEL_JAMIRA_API.Models.Cadastros;
using FEL_JAMIRA_API.Models.MultiModelacao;
using FEL_JAMIRA_API.Util;
using FEL_JAMIRA_WEB_API.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FEL_JAMIRA_API.Controllers
{
    public class ClientesController : GenericController<Cliente>
    {
        // POST: Generic/Cadastrar
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Método para cadastrar um novo registro.
        /// </summary>
        /// <param name="entidade"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("~/api/Clientes/CadastrarCliente")]
        public async Task<ResponseViewModel<Usuario>> CadastrarCliente(CadastroCliente cadastroCliente)
        {
            try
            {
                Usuario existente = new Usuario();
                Pessoa existente1 = new Pessoa();

                Task.Run(async () => {
                    var valor = db.Usuarios.Where(x => x.Login == cadastroCliente.Email).FirstOrDefault();
                    existente = valor;

                    var valor2 = db.Pessoas.Where(x => x.CPF == cadastroCliente.CPF).FirstOrDefault();
                    existente1 = valor2;
                }).Wait();

                if (existente != null)
                    throw new Exception("Email already in use");

                if (existente1 != null)
                    throw new Exception("Individual Registration already in use");

                UsuariosController usuariosController = new UsuariosController();

                string auxSenha = Helpers.GenerateRandomString();

                Usuario usuario = new Usuario
                {
                    Login = cadastroCliente.Email,
                    AuxSenha = auxSenha,
                    Senha = Helpers.CriarSenha(cadastroCliente.Senha, auxSenha),
                    Level = 2,
                    Nome = cadastroCliente.Nickname ?? "",
                    Foto = cadastroCliente.Foto,
                    Pessoa = new Cliente
                    {
                        Nome = cadastroCliente.Nome,
                        Nascimento = cadastroCliente.Nascimento,
                        CPF = cadastroCliente.CPF ?? "",
                        RG = cadastroCliente.RG ?? "",
                        Nickname = cadastroCliente.Nickname,
                        DataCriacao = DateTime.Now,
                        Saldo = 0,
                        Deletado = false,
                        EnderecoPessoa = new Endereco
                        {
                            Rua = cadastroCliente.Rua ?? "",
                            Numero = cadastroCliente.Numero,
                            Bairro = cadastroCliente.Bairro ?? "",
                            CEP = cadastroCliente.CEP ?? "",
                            Complemento = cadastroCliente.Complemento ?? "",
                            IdCidade = cadastroCliente.IdCidade,
                            IdEstado = cadastroCliente.IdEstado
                        }
                    }
                };

                db.Usuarios.Add(usuario);

                Task.Run(async () => {
                    await db.SaveChangesAsync();
                }).Wait();

                ResponseViewModel<Usuario> response = new ResponseViewModel<Usuario> {
                    Data = usuario,
                    Sucesso = true,
                    Serializado = true,
                    Mensagem = "Cadastro Realizado com Sucesso!"
                };

                return response;
            }
            catch (Exception e)
            {
                return new ResponseViewModel<Usuario>
                {
                    Data = null,
                    Sucesso = false,
                    Serializado = true,
                    Mensagem = "We were unable to fulfill your request: " + e.Message
                };
            }
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("~/api/Clientes/EditarCliente")]
        public async Task<ResponseViewModel<Usuario>> EditarCliente(DadosCliente editarCliente)
        {
            try
            {
                DadosGeraisCliente dados = new DadosGeraisCliente();

                Task.Run(async () =>
                {
                    DadosGeraisCliente retorno = db.Usuarios.Join(db.Clientes,
                        a => a.IdPessoa,
                        b => b.Id,
                        (a, b) => new { a, b }).
                            Join(db.Enderecos,
                            a2 => a2.b.IdEndereco,
                            eP => eP.Id,
                            (a2, eP) => new { a2, eP }).
                                    Select((x) => new DadosGeraisCliente
                                    {
                                        usuario = x.a2.a,
                                        cliente = x.a2.b,
                                        endereco = x.eP
                                    }).FirstOrDefault(x => x.usuario.Login.Equals(editarCliente.Email));

                    dados = retorno;
                }).Wait();

                if (dados == null)
                    throw new Exception("Data not found. please if you're a user, contact me. it's a bug or yout trying to hack me.");

                UsuariosController usuariosController = new UsuariosController();
                ClientesController estacionamentosController = new ClientesController();

                Usuario usuario = dados.usuario;
                usuario.Nome = editarCliente.Nickname;

                Endereco endereco = dados.endereco;
                endereco.Bairro = editarCliente.Bairro;
                endereco.CEP = editarCliente.CEP;
                endereco.IdCidade = editarCliente.IdCidade;
                endereco.IdEstado = editarCliente.IdEstado;
                endereco.Rua = editarCliente.Rua;
                endereco.Numero = editarCliente.Numero;
                endereco.Complemento = editarCliente.Complemento;

                Task.Run(async () =>
                {
                    db.Entry(dados.usuario).State = EntityState.Detached;
                    db.Entry(dados.endereco).State = EntityState.Detached;

                    db.Entry(usuario).State = EntityState.Modified;
                    db.Entry(endereco).State = EntityState.Modified;

                    await db.SaveChangesAsync();
                }).Wait();


                ResponseViewModel<Usuario> responseUser = new ResponseViewModel<Usuario>
                {
                    Mensagem = "Sucessfull registered!",
                    Serializado = true,
                    Sucesso = true,
                    Data = usuario
                };
                return responseUser;
            }
            catch (Exception e)
            {
                return new ResponseViewModel<Usuario>()
                {
                    Data = null,
                    Serializado = true,
                    Sucesso = false,
                    Mensagem = "Sorry, something went wrong: " + e.Message
                };
            }
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("~/api/Clientes/BuscarCliente/{id}")]
        public async Task<ResponseViewModel<Cliente>> BuscarCliente(int? id)
        {
            try
            {
                if (id != null)
                {
                    Cliente entidade =
                        db.Clientes.Include("EnderecoPessoa").Where(x => x.Id == id).FirstOrDefault();

                    return new ResponseViewModel<Cliente>()
                    {
                        Data = entidade,
                        Serializado = true,
                        Sucesso = true,
                        Mensagem = "Search carried out successfully."
                    };
                }
                else
                {
                    return new ResponseViewModel<Cliente>()
                    {
                        Data = null,
                        Serializado = true,
                        Sucesso = true,
                        Mensagem = "No customer search filter."
                    };
                }
            }
            catch (Exception e)
            {
                return new ResponseViewModel<Cliente>()
                {
                    Data = null,
                    Serializado = true,
                    Sucesso = false,
                    Mensagem = "We were unable to fulfill your request: " + e.Message
                };
            }
        }
    }
}