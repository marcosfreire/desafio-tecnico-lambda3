using DesafioTecnico.Lamda3.Domain.BaseClasses;
using FluentValidation;
using FluentValidation.Results;
using System;

namespace DesafioTecnico.Lamda3.Domain
{
    public class Professor
    {
        public Professor(int id, string nome, string sobrenome, DateTime dataNascimento)
        {
            Id = id;
            Nome = nome;
            Sobrenome = sobrenome;
            DataNascimento = dataNascimento;
        }

        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Sobrenome { get; private set; }
        public DateTime DataNascimento { get; private set; }

        public void AtualizarDados(Professor professor)
        {
            Nome = professor.Nome;
            Sobrenome = professor.Sobrenome;
            DataNascimento = professor.DataNascimento;
        }
    }

    public class ValidadorProfessor : FluentValidationBaseClass<Professor>
    {
        public ValidadorProfessor()
        {
            RuleFor(a => a.Nome)
               .NotEmpty().WithMessage("O campo Nome é obrigatório.")
               .MaximumLength(100).WithMessage("O campo Nome deve possuir no máximo 100 caracteres.");

            RuleFor(a => a.Sobrenome)
               .NotEmpty().WithMessage("O campo Sobrenome é obrigatório.")
               .MaximumLength(100).WithMessage("O campo Sobrenome deve possuir no máximo 100 caracteres.");

            RuleFor(a => a.DataNascimento)
                    .GreaterThan(DateTime.MinValue).WithMessage("O campo Data de Nascimento é obrigatório.")
                    .LessThan(DateTime.Now)
                    .WithMessage("A Data de Nascimento deve ser inferior a data atual.");

            ValidationResult = new ValidationResult();

            //RuleForEach(a => a.Disciplinas).SetValidator(new CourseValidator());
            //RuleFor(r => r.Disciplinas).Must(PossuirAoMenosUmaDisciplinaAtiva).WithMessage("É necessário existir ao menos um curso ativo");
        }

        public bool Valido(Professor professor)
        {
            ValidationResult = Validate(professor);
            return ValidationResult.IsValid;
        }
    }
}
