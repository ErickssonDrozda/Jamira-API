using FEL_JAMIRA_API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FEL_JAMIRA_WEB_API.Models
{
    public class Context : DbContext
    {

        public Context()
                : base("FEL_JAMIRA_DB")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Configure default schema
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<Carro>().ToTable("CARROS");
            modelBuilder.Entity<CategoriaFaleConosco>().ToTable("CATEGORIA_FALE_CONOSCO");
            modelBuilder.Entity<Cidade>().ToTable("CIDADE");
            modelBuilder.Entity<Cliente>().ToTable("CLIENTE");
            modelBuilder.Entity<CompraCreditos>().ToTable("COMPRA_CREDITOS");
            modelBuilder.Entity<Endereco>().ToTable("ENDERECO");
            modelBuilder.Entity<Estacionamento>().ToTable("ESTACIONAMENTO");
            modelBuilder.Entity<Estado>().ToTable("ESTADO");
            modelBuilder.Entity<FaleConosco>().ToTable("FALE_CONOSCO");
            modelBuilder.Entity<Marca>().ToTable("MARCA");
            modelBuilder.Entity<Pessoa>().ToTable("PESSOA");
            modelBuilder.Entity<Solicitacao>().ToTable("SOLICITACAO");
            modelBuilder.Entity<Usuario>().ToTable("USUARIO");
            modelBuilder.Entity<TipoConta>().ToTable("TIPO_CONTA");
            modelBuilder.Entity<Banco>().ToTable("BANCO");
            modelBuilder.Entity<ContaDeposito>().ToTable("CONTA_DEPOSITO");

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<Estacionamento>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }

        public DbSet<Carro> Carros { get; set; }
        public DbSet<CategoriaFaleConosco> CategoriasFaleConosco { get; set; }
        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<CompraCreditos> CompraCreditos { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Estacionamento> Estacionamentos { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<FaleConosco> FaleConosco { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Solicitacao> Solicitacoes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ContaDeposito> ContaDepositos { get; set; }
        public DbSet<Banco> Bancos { get; set; }
        public DbSet<TipoConta> TipoContas { get; set; }

    }
}
