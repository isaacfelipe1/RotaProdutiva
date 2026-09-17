using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RotaProdutiva.Domain.Entities;

namespace RotaProdutiva.Infrastructure.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Nome)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.SenhaHash)
                .IsRequired();

            builder.Property(u => u.WhatsApp)
                .HasMaxLength(20);

            builder.Property(u => u.Tipo)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(u => u.StatusAprovacao)
                .IsRequired()
                .HasConversion<int>();

            builder.HasMany(u => u.Cursos)
                .WithOne(c => c.Tutor)
                .HasForeignKey(c => c.TutorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
