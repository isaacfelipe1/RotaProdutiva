using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RotaProdutiva.Domain.Entities;

namespace RotaProdutiva.Infrastructure.Data.Configurations
{
    public class InscricaoConfiguration : IEntityTypeConfiguration<Inscricao>
    {
        public void Configure(EntityTypeBuilder<Inscricao> builder)
        {
            builder.ToTable("Inscricoes");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Nome)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(i => i.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(i => i.WhatsApp)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(i => i.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.HasIndex(i => new { i.Email, i.CursoId });
        }
    }
}
