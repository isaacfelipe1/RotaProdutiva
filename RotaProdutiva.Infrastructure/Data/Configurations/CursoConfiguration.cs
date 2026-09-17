using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RotaProdutiva.Domain.Entities;

namespace RotaProdutiva.Infrastructure.Data.Configurations
{
    public class CursoConfiguration : IEntityTypeConfiguration<Curso>
    {
        public void Configure(EntityTypeBuilder<Curso> builder)
        {
            builder.ToTable("Cursos");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Descricao)
                .IsRequired();

            builder.Property(c => c.DataInicio)
                .IsRequired();

            builder.Property(c => c.CargaHoraria)
                .IsRequired();

            builder.Property(c => c.Vagas)
                .IsRequired();

            builder.Property(c => c.Modalidade)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(c => c.Local)
                .HasMaxLength(300);

            builder.Ignore(c => c.VagasDisponiveis);

            builder.HasMany(c => c.Inscricoes)
                .WithOne(i => i.Curso)
                .HasForeignKey(i => i.CursoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
