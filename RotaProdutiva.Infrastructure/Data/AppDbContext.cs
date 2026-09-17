using Microsoft.EntityFrameworkCore;
using RotaProdutiva.Domain.Entities;

namespace RotaProdutiva.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Curso> Cursos => Set<Curso>();
        public DbSet<Inscricao> Inscricoes => Set<Inscricao>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
