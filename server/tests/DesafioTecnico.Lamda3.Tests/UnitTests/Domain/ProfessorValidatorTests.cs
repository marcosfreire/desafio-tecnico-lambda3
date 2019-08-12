using DesafioTecnico.Lamda3.Domain;
using System;
using Xunit;

namespace DesafioTecnico.Lamda3.Tests.Domain
{   
    public class ProfessorValidatorTests
    {
        private ValidadorProfessor _validador;

        private Professor _professor = new Professor(1, "Nome Professor Valido", "Sobrenome", new DateTime(1990, 8, 28));

        public ProfessorValidatorTests()
        {
            _validador = new ValidadorProfessor();
        }

        [Trait("TodosOsDadosCorretos", "ProfessorValido")]
        [Fact]
        public void Valido_TodosOsDadosCorreto_ProfessorValido()
        {
            var valido = _validador.Valido(_professor);

            Assert.True(valido);
            Assert.Empty(_validador.ErrorMessages);
        }

        [Trait("NomeProfessorVazio", "ProfessorInvalido")]
        [Fact]
        public void Valido_NomeProfessorVazio_ProfessorInvalido()
        {
            _professor = new Professor(1, "", "Sobrenome", new DateTime(1990, 8, 28));

            var valido = _validador.Valido(_professor);

            Assert.False(valido);
            Assert.Single(_validador.ErrorMessages);
            Assert.Contains("O campo Nome é obrigatório.", _validador.ErrorMessages);
        }

        [Trait("NomeProfessorNull", "ProfessorInvalido")]
        [Fact]
        public void Valido_NomeProfessorNull_ProfessorInvalido()
        {
            _professor = new Professor(1, null, "Sobrenome", new DateTime(1990, 8, 28));

            var valido = _validador.Valido(_professor);

            Assert.False(valido);
            Assert.Single(_validador.ErrorMessages);
            Assert.Contains("O campo Nome é obrigatório.", _validador.ErrorMessages);
        }

        [Trait("SobrenomeProfessorVazio", "ProfessorInvalido")]
        [Fact]
        public void Valido_SobrenomeProfessorVazio_ProfessorInvalido()
        {
            _professor = new Professor(1, "Nome", "", new DateTime(1990, 8, 28));

            var valido = _validador.Valido(_professor);

            Assert.False(valido);
            Assert.Single(_validador.ErrorMessages);
            Assert.Contains("O campo Sobrenome é obrigatório.", _validador.ErrorMessages);
        }

        [Trait("SobrenomeProfessorNull", "ProfessorInvalido")]
        [Fact]
        public void Valido_SobrenomeProfessorNulll_ProfessorInvalido()
        {
            _professor = new Professor(1, "Nome", null, new DateTime(1990, 8, 28));

            var valido = _validador.Valido(_professor);

            Assert.False(valido);
            Assert.Single(_validador.ErrorMessages);
            Assert.Contains("O campo Sobrenome é obrigatório.", _validador.ErrorMessages);
        }

        [Trait("NomeMaior100Caracteres", "ProfessorInvalido")]
        [Fact]
        public void Valido_NomeMaior100Caracteres_ProfessorInvalido()
        {
            const string TEXTO_MAIOR_100_CARACTERES = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

            _professor = new Professor(1, TEXTO_MAIOR_100_CARACTERES, "Sobrenome", new DateTime(1990, 8, 28));
            
            var valido = _validador.Valido(_professor);

            Assert.False(valido);
            Assert.Single(_validador.ErrorMessages);
            Assert.Contains("O campo Nome deve possuir no máximo 100 caracteres.", _validador.ErrorMessages);
        }

        [Trait("SobrenomeMaior100Caracteres", "ProfessorInvalido")]
        [Fact]
        public void Valido_SobrenomeMaior100Caracteres_ProfessorInvalido()
        {
            const string TEXTO_MAIOR_100_CARACTERES = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

            _professor = new Professor(1, "Nome", TEXTO_MAIOR_100_CARACTERES, new DateTime(1990, 8, 28));

            var valido = _validador.Valido(_professor);

            Assert.False(valido);
            Assert.Single(_validador.ErrorMessages);
            Assert.Contains("O campo Sobrenome deve possuir no máximo 100 caracteres.", _validador.ErrorMessages);
        }
    }
}
