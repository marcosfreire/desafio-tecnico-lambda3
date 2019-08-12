using DesafioTecnico.Lamda3.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioTecnico.Lamda3.Repository.EntityTypeConfiguration
{
    public class ProfessorEntityTypeConfiguration : IEntityTypeConfiguration<Professor>
    {
        public void Configure(EntityTypeBuilder<Professor> builder)
        {
            builder.ToTable("Professor");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id).ValueGeneratedOnAdd();

            builder.Property(a => a.Nome).HasMaxLength(100);
            builder.Property(a => a.Sobrenome).HasMaxLength(100);
            builder.Property(a => a.DataNascimento);
        }
    }
}
