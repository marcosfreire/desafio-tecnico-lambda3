using DesafioTecnico.Lamda3.Domain;
using DesafioTecnico.Lamda3.Repository.Context;
using System.Collections.Generic;
using System.Linq;

namespace DesafioTecnico.Lamda3.Repository
{
    public interface IProfessorRepository
    {
        ICollection<Professor> BuscarTodos();
        Professor BuscarPorId(int id);
        void Adicionar(Professor professor);
        void Atualizar(Professor professor);
        void Remover(Professor professor);
    }

    public class ProfessorRepository : IProfessorRepository
    {
        private readonly ApplicationDataContext _context;

        public ProfessorRepository(ApplicationDataContext context)
        {
            _context = context;
        }

        public ICollection<Professor> BuscarTodos()
        {
            return _context.Professores.ToList();
        }

        public Professor BuscarPorId(int id)
        {
            return _context.Professores.FirstOrDefault(a => a.Id == id);
        }

        public void Adicionar(Professor professor)
        {
            _context.Professores.Add(professor);
            _context.SaveChanges();
        }

        public void Atualizar(Professor professor)
        {
            _context.Professores.Update(professor);
            _context.SaveChanges();
        }

        public void Remover(Professor professor)
        {   
            _context.Professores.Remove(professor);
            _context.SaveChanges();
        }
    }
}
