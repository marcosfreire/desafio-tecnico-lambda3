using DesafioTecnico.Lamda3.Domain;
using Xunit;

namespace DesafioTecnico.Lamda3.Tests
{
    public class ValidadorDisciplinaTests
    {
        private ValidadorDisciplina _validador;

        private Disciplina _disciplina = new Disciplina(1,"Matematica");

        public ValidadorDisciplinaTests()
        {
            _validador = new ValidadorDisciplina();
        }

        [Trait("TodosOsDadosCorretos", "DisciplinaValida")]
        [Theory]
        [InlineData(1,"Matematica")]
        [InlineData(2, "Ciencias")]
        [InlineData(3, "Biologia")]
        public void Valido_TodosOsDadosCorreto_DisciplinaValida(int id , string disciplina)
        {
            _disciplina = new Disciplina(id, disciplina);
            var valido = _validador.Valido(_disciplina);

            Assert.True(valido);
            Assert.Empty(_validador.ErrorMessages);

        }

        [Trait("NomeDisciplinaIncorreto", "DisciplinaInvalida")]
        [Theory]
        [InlineData(1, "")]
        [InlineData(2, null)]
        public void Valido_NomeDisciplinaIncorreto_DisciplinaInvalida(int id, string disciplina)
        {
            _disciplina = new Disciplina(id, disciplina);
            var valido = _validador.Valido(_disciplina);

            Assert.False(valido);
            Assert.Single(_validador.ErrorMessages);
            Assert.Contains("O campo Nome é obrigatório.", _validador.ErrorMessages);
        }

        [Trait("NomeMaior100Caracteres", "DisciplinaInvalida")]
        [Fact]
        public void Valido_NomeMaior100Caracteres_ProfessorInvalido()
        {
            const string TEXTO_MAIOR_100_CARACTERES = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

            _disciplina = new Disciplina(1, TEXTO_MAIOR_100_CARACTERES);

            var valido = _validador.Valido(_disciplina);

            Assert.False(valido);
            Assert.Single(_validador.ErrorMessages);
            Assert.Contains("O campo Nome deve possuir no máximo 100 caracteres.", _validador.ErrorMessages);
        }
    }
}
