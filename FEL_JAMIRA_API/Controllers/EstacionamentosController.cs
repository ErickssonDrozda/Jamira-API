using FEL_JAMIRA_API.Models;
using FEL_JAMIRA_API.Models.Cadastros;
using FEL_JAMIRA_API.Models.Estacionamentos;
using FEL_JAMIRA_API.Models.MultiModelacao;
using FEL_JAMIRA_API.Util;
using FEL_JAMIRA_WEB_API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace FEL_JAMIRA_API.Controllers
{
    public class EstacionamentosController : GenericController<Estacionamento>
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
        [System.Web.Http.Route("~/api/Estacionamentos/CadastrarFornecedor")]
        public async Task<ResponseViewModel<Usuario>> CadastrarFornecedor(CadastroFornecedor cadastroFornecedor)
        {
            try
            {
                Usuario existente = new Usuario();
                Pessoa existente1 = new Pessoa();

                Task.Run(async () => {
                    var valor = db.Usuarios.Where(x => x.Login == cadastroFornecedor.Email).FirstOrDefault();
                    existente = valor;

                    var valor2 = db.Pessoas.Where(x => x.CPF == cadastroFornecedor.CPF).FirstOrDefault();
                    existente1 = valor2;
                }).Wait();

                if (existente != null)
                    throw new Exception("Email already in use");

                if (existente1 != null)
                    throw new Exception("Individual Registration already in use");

                UsuariosController usuariosController = new UsuariosController();
                EstacionamentosController estacionamentosController = new EstacionamentosController();

                string auxSenha = Helpers.GenerateRandomString();

                Usuario usuario = new Usuario
                {
                    Login = cadastroFornecedor.Email,
                    AuxSenha = auxSenha,
                    Senha = Helpers.CriarSenha(cadastroFornecedor.Senha, auxSenha),
                    Level = 1,
                    Nome = cadastroFornecedor.Nickname ?? "",
                    Foto = cadastroFornecedor.Foto,
                    Pessoa = new Pessoa
                    {
                        Nome = cadastroFornecedor.NomeProprietario,
                        Nascimento = cadastroFornecedor.Nascimento,
                        CPF = cadastroFornecedor.CPF ?? "",
                        RG = cadastroFornecedor.RG ?? "",
                        DataCriacao = DateTime.Now,
                        EnderecoPessoa = new Endereco
                        {
                            Rua = cadastroFornecedor.Rua ?? "",
                            Numero = cadastroFornecedor.Numero,
                            Bairro = cadastroFornecedor.Bairro ?? "",
                            CEP = cadastroFornecedor.CEP ?? "",
                            Complemento = cadastroFornecedor.Complemento ?? "",
                            IdCidade = cadastroFornecedor.IdCidade,
                            IdEstado = cadastroFornecedor.IdEstado
                        }
                    }
                };

                db.Usuarios.Add(usuario);

                Task.Run(async () => {
                    await db.SaveChangesAsync();
                }).Wait();

                
                Estacionamento estacionamento = new Estacionamento
                {
                    NomeEstacionamento = cadastroFornecedor.NomeEstacionamento ?? "",
                    CNPJ = cadastroFornecedor.CNPJ ?? "",
                    InscricaoEstadual = cadastroFornecedor.InscricaoEstadual ?? "",
                    IdEnderecoEstabelecimento = null,
                    Deletado = false,
                    IdPessoa = usuario.IdPessoa,
                    ValorHora = cadastroFornecedor.Value
                };

                db.Estacionamentos.Add(estacionamento);

                Task.Run(async () => {
                    await db.SaveChangesAsync();
                }).Wait();

                ContaDeposito contaDeposito = new ContaDeposito 
                {
                    Agencia = cadastroFornecedor.Agencia,
                    IdBanco = cadastroFornecedor.IdBanco,
                    IdTipoConta = cadastroFornecedor.IdTipoConta,
                    Conta = cadastroFornecedor.Conta,
                    IdEstacionamento = estacionamento.Id
                };

                db.ContaDepositos.Add(contaDeposito);

                Task.Run(async () => {
                    await db.SaveChangesAsync();
                }).Wait();

                ResponseViewModel<Usuario> responseUser = new ResponseViewModel<Usuario>
                {
                    Mensagem = "Registration Successful!",
                    Serializado = true,
                    Sucesso = true,
                    Data =  usuario
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
                    Mensagem = "We were unable to fulfill your request: " + e.Message
                };
            }
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("~/api/Estacionamentos/EditarFornecedor")]
        public async Task<ResponseViewModel<Usuario>> EditarFornecedor(DadosEstacionamento editarFornecedor)
        {
            try
            {
                DadosGeraisEstacionamento dados = new DadosGeraisEstacionamento();

                Task.Run(async () =>
                {
                    DadosGeraisEstacionamento retorno = db.Usuarios.Join(db.Estacionamentos,
                        a => a.IdPessoa,
                        b => b.IdPessoa,
                        (a, b) => new { a, b }).
                            Join(db.Pessoas,
                            a2 => a2.a.IdPessoa,
                            c => c.Id,
                            (a2, c) => new { a2, c }).
                                Join(db.Enderecos,
                                a3 => a3.c.IdEndereco,
                                eP => eP.Id,
                                (a3, eP) => new { a3, eP }).
                                    Join(db.ContaDepositos,
                                    a4 => a4.a3.a2.b.Id,
                                    cD => cD.IdEstacionamento,
                                    (a4, cD) => new { a4, cD }).
                                        Select((x) => new DadosGeraisEstacionamento
                                        {
                                            usuario = x.a4.a3.a2.a,
                                            enderecoUsuario = x.a4.eP,
                                            estacionamento = x.a4.a3.a2.b,
                                            pessoa = x.a4.a3.c,
                                            contaDeposito = x.cD
                                        }).FirstOrDefault(x => x.usuario.Login.Equals(editarFornecedor.Email));

                    dados = retorno;

                }).Wait();

                if (dados == null)
                    throw new Exception("Data not found. please if you're a user, contact me. it's a bug or yout trying to hack me.");

                UsuariosController usuariosController = new UsuariosController();
                EstacionamentosController estacionamentosController = new EstacionamentosController();
                Usuario usuario = dados.usuario;
                usuario.Nome = editarFornecedor.Nickname;

                Estacionamento estacionamento = dados.estacionamento;
                estacionamento.InscricaoEstadual = editarFornecedor.InscricaoEstadual;
                estacionamento.ValorHora = editarFornecedor.ValorHora;

                Endereco endereco = dados.enderecoUsuario;
                endereco.Bairro = editarFornecedor.Bairro;
                endereco.CEP = editarFornecedor.CEP;
                endereco.IdCidade = editarFornecedor.IdCidade;
                endereco.IdEstado = editarFornecedor.IdEstado;
                endereco.Rua = editarFornecedor.Rua;
                endereco.Numero = editarFornecedor.Numero;
                endereco.Complemento = editarFornecedor.Complemento;

                ContaDeposito contaDeposito = dados.contaDeposito;
                contaDeposito.Agencia = editarFornecedor.Agencia;
                contaDeposito.IdBanco = editarFornecedor.IdBanco;
                contaDeposito.IdTipoConta = editarFornecedor.IdTipoConta;
                contaDeposito.Agencia = editarFornecedor.Agencia;
                contaDeposito.Conta = editarFornecedor.Conta;

                Task.Run(async () =>
                {
                    db.Entry(dados.usuario).State = EntityState.Detached;
                    db.Entry(dados.enderecoUsuario).State = EntityState.Detached;
                    db.Entry(dados.estacionamento).State = EntityState.Detached;
                    db.Entry(dados.contaDeposito).State = EntityState.Detached;

                    db.Entry(usuario).State = EntityState.Modified;
                    db.Entry(estacionamento).State = EntityState.Modified;
                    db.Entry(endereco).State = EntityState.Modified;
                    db.Entry(contaDeposito).State = EntityState.Modified;

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
        [System.Web.Http.Route("~/api/Estacionamentos/EstacionamentoPorPessoa")]
        public async Task<ResponseViewModel<Estacionamento>> GetEstacionamentoPorPessoa(int IdPessoa)
        {
            try
            {
                Estacionamento entidade =
                    db.Estacionamentos.Include("EnderecoEstacionamento").Include("Proprietario").Include("Proprietario.EnderecoPessoa").Where(x => x.IdPessoa.Equals(IdPessoa)).FirstOrDefault();

                return new ResponseViewModel<Estacionamento>()
                {
                    Data = entidade,
                    Serializado = true,
                    Sucesso = true,
                    Mensagem = "Busca realizada com sucesso"
                };
            }
            catch (Exception e)
            {
                return new ResponseViewModel<Estacionamento>()
                {
                    Data = null,
                    Serializado = true,
                    Sucesso = false,
                    Mensagem = "Não foi possivel atender a sua solicitação: " + e.Message
                };
            }
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("~/api/Estacionamentos/GetContaDepositoPorPessoa")]
        public async Task<ResponseViewModel<ContaDeposito>> GetContaDepositoPorPessoa(int IdPessoa)
        {
            try
            {
                ContaDeposito entidade =
                    db.ContaDepositos.Include("Estacionamento").Where(x => x.Estacionamento.IdPessoa.Equals(IdPessoa)).FirstOrDefault();

                return new ResponseViewModel<ContaDeposito>()
                {
                    Data = entidade,
                    Serializado = true,
                    Sucesso = true,
                    Mensagem = "Busca realizada com sucesso"
                };
            }
            catch (Exception e)
            {
                return new ResponseViewModel<ContaDeposito>()
                {
                    Data = null,
                    Serializado = true,
                    Sucesso = false,
                    Mensagem = "Não foi possivel atender a sua solicitação: " + e.Message
                };
            }
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("~/api/Estacionamentos/GetBancos")]
        public async Task<ResponseViewModel<List<Banco>>> GetBancos()
        {
            try
            {
                var response = new ResponseViewModel<List<Banco>>
                {
                    Data = await db.Set<Banco>().AsQueryable().ToListAsync(),
                    Sucesso = true,
                    Mensagem = "Dados retornados com sucesso."
                };
                return response;
            }
            catch (Exception e)
            {
                return new ResponseViewModel<List<Banco>>()
                {
                    Data = null,
                    Serializado = true,
                    Sucesso = false,
                    Mensagem = "Não foi possivel atender a sua solicitação: " + e.Message
                };
            }

        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("~/api/Estacionamentos/GetTipoContas")]
        public async Task<ResponseViewModel<List<TipoConta>>> GetTipoContas()
        {
            try
            {
                var response = new ResponseViewModel<List<TipoConta>>
                {
                    Data = await db.Set<TipoConta>().AsQueryable().ToListAsync(),
                    Sucesso = true,
                    Mensagem = "Dados retornados com sucesso."
                };
                return response;
            }
            catch (Exception e)
            {
                return new ResponseViewModel<List<TipoConta>>()
                {
                    Data = null,
                    Serializado = true,
                    Sucesso = false,
                    Mensagem = "Não foi possivel atender a sua solicitação: " + e.Message
                };
            }

        }

        [System.Web.Http.Authorize]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("~/api/Estacionamentos/EstacionamentoDisponiveis")]
        public async Task<ResponseViewModel<List<DadosRapidosEstacionamento>>> GetEstacionamentosDisponiveis()
        {
            try
            {
                List<DadosRapidosEstacionamento> retorno = db.Usuarios.
                    Join(db.Estacionamentos,
                    a => a.IdPessoa,
                    b => b.IdPessoa,
                    (a, b) => new { a, b }).Where(x => x.b.TemEstacionamento && x.b.ValorHora > 0).
                        Join(db.Pessoas,
                        a2 => a2.a.IdPessoa,
                        c => c.Id,
                        (a2, c) => new { a2, c }).
                            Join(db.Enderecos.Include("Cidade").Include("Estado"),
                            a3 => a3.a2.b.IdEnderecoEstabelecimento,
                            eP => eP.Id,
                            (a3, eP) => new { a3, eP }).
                                    Select((x) => new DadosRapidosEstacionamento
                                    {
                                        idEstacionamento = x.a3.a2.b.Id,
                                        Foto = x.a3.a2.a.Foto,
                                        Nome = x.a3.a2.b.NomeEstacionamento,
                                        ValorHr = x.a3.a2.b.ValorHora,
                                        Endereco = (x.eP.Rua + ", " + x.eP.Bairro + ", " + x.eP.Cidade.NomeCidade + " - " + x.eP.Estado.Sigla).ToString()
                                    }).ToList();

                return new ResponseViewModel<List<DadosRapidosEstacionamento>>()
                {
                    Data = retorno,
                    Serializado = true,
                    Sucesso = true,
                    Mensagem = "Search carried out successfully."
                };
            }
            catch (Exception e)
            {
                return new ResponseViewModel<List<DadosRapidosEstacionamento>>()
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
