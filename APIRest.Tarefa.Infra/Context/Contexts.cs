using APIRest.Tarefa.Utility;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace APIRest.Tarefa.Infra.Context
{
    public class Contexts : DbContext
    {
        private readonly string ConexaoBanco;

        public Contexts() { ConexaoBanco = ConfigAppSettings.StringConnection(); }

        public Contexts(DbContextOptions<Contexts> options) : base (options) { }

        public virtual DbSet<Domain.Entity.Tarefa> Tarefa { get; set; }
        public virtual DbSet<Domain.Entity.Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConexaoBanco);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.HasDefaultSchema("dbo");
        }
    }
}
