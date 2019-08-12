using DesafioTecnico.Lamda3.Domain;
using DesafioTecnico.Lamda3.Repository.Context;
using System.Collections.Generic;
using System.Linq;

namespace DesafioTecnico.Lamda3.Repository
{
    public interface IDisciplinaRepository
    {
        ICollection<Disciplina> BuscarTodos();
        Disciplina BuscarPorId(int id);
        void Adicionar(Disciplina disciplina);
        void Remover(Disciplina disciplina);
    }

    public class DisciplinaRepository : IDisciplinaRepository
    {
        private readonly ApplicationDataContext _context;

        public DisciplinaRepository(ApplicationDataContext context)
        {
            _context = context;
        }

        public ICollection<Disciplina> BuscarTodos()
        {
            return _context.Disciplinas.ToList();
        }

        public Disciplina BuscarPorId(int id)
        {
            return _context.Disciplinas.FirstOrDefault(a => a.Id == id);
        }

        public void Adicionar(Disciplina disciplina)
        {
            _context.Disciplinas.Add(disciplina);
            _context.SaveChanges();
        }

        public void Remover(Disciplina disciplina)
        {
            _context.Disciplinas.Remove(disciplina);
            _context.SaveChanges();
        }
    }
}
