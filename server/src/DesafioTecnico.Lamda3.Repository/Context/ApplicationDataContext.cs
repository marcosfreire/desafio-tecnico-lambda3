using Microsoft.EntityFrameworkCore;
using DesafioTecnico.Lamda3.Repository.EntityTypeConfiguration;
using DesafioTecnico.Lamda3.Domain;

namespace DesafioTecnico.Lamda3.Repository.Context
{
    public class ApplicationDataContext : DbContext
    {
        public DbSet<Professor> Professores { get; set; }
        public DbSet<Disciplina> Disciplinas { get; set; }

        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ProfessorEntityTypeConfiguration());
            builder.ApplyConfiguration(new DisciplinaEntityTypeConfiguration());
        }
    }
}
