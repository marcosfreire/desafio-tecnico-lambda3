using AutoMapper;
using DesafioTecnico.Lambda3.Api.Models;
using DesafioTecnico.Lamda3.Domain;

namespace DesafioTecnico.Lamda3.Tests.Controller
{
    public class BaseTests
    {
        protected readonly IMapper Mapper;

        public BaseTests()
        {
            Mapper = new MapperConfiguration(opts =>
            {
                opts.CreateMap<ProfessorModel, Professor>()
                   .ConstructUsing(a => new Professor(a.Id, a.Nome, a.Sobrenome, a.DataNascimento));

                opts.CreateMap<Professor, ProfessorModel>();

                opts.CreateMap<DisciplinaModel, Disciplina>()
                     .ConstructUsing(a => new Disciplina(a.Id, a.Nome));

                opts.CreateMap<Disciplina, DisciplinaModel>();
            }).CreateMapper();

        }
    }
}
