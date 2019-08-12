using DesafioTecnico.Lamda3.Domain.BaseClasses;
using FluentValidation;

namespace DesafioTecnico.Lamda3.Domain
{
    public class Disciplina
    {
        public Disciplina(int id, string nome)
        {
            Id = id;
            Nome = nome;
        }

        public int Id { get; private set; }
        public string Nome { get; private set; }        
    }

    public class ValidadorDisciplina : FluentValidationBaseClass<Disciplina>
    {
        public ValidadorDisciplina()
        {
            RuleFor(a => a.Nome)
               .NotEmpty().WithMessage("O campo Nome é obrigatório.")
               .MaximumLength(100).WithMessage("O campo Nome deve possuir no máximo 100 caracteres.");
        }

        public bool Valido(Disciplina disciplina)
        {
            ValidationResult = Validate(disciplina);
            return ValidationResult.IsValid;
        }
    }
}