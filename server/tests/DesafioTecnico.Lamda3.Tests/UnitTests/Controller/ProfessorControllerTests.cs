using DesafioTecnico.Lambda3.Api.Controllers;
using DesafioTecnico.Lambda3.Api.Models;
using DesafioTecnico.Lamda3.Domain;
using DesafioTecnico.Lamda3.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Net;
using Xunit;

namespace DesafioTecnico.Lamda3.Tests.Controller
{
    public class ProfessorControllerTests : BaseTests
    {
        public ProfessorControllerTests()
        {

        }

        private ProfessorController _professorController;
        private readonly ValidadorProfessor _validator = new ValidadorProfessor();

        private readonly Mock<IProfessorRepository> _repository = new Mock<IProfessorRepository>();
        private readonly Mock<ILogger<ProfessorController>> _logger = new Mock<ILogger<ProfessorController>>();

        private readonly ProfessorModel _professorModel = new ProfessorModel
        {
            DataNascimento = new DateTime(1990, 08, 28),
            Id = 1,
            Nome = "Marcos",
            Sobrenome = "Freire"
        };

        #region Adicionar professor - POST

        [Trait("ProfessorController", "Post_DadosCorretos_AdicionarProfessorComSucesso")]
        [Fact]
        public void Post_DadosCorretos_AdicionarProfessorComSucesso()
        {
            _professorController = new ProfessorController(_repository.Object, _validator, Mapper, _logger.Object);

            var response = _professorController.Post(_professorModel) as OkObjectResult;
            var responseContent = response.Value as ApiResponse;

            Assert.True(responseContent.Success);
            Assert.Null(responseContent.Errors);
            Assert.Null(responseContent.ApplicationError);
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);

            _repository.Verify(a => a.Adicionar(It.IsAny<Professor>()), Times.Once);
        }

        [Trait("ProfessorController", "Post_CamposObrigatoriosNaoInformados_BadRequest")]
        [Fact]
        public void Post_CamposObrigatoriosNaoInformados_BadRequest()
        {
            _professorController = new ProfessorController(_repository.Object, _validator, Mapper, _logger.Object);

            _professorModel.Nome = string.Empty;
            _professorModel.Sobrenome = string.Empty;
            _professorModel.DataNascimento = DateTime.MinValue;

            var response = _professorController.Post(_professorModel) as BadRequestObjectResult;
            var responseContent = response.Value as ApiResponse;

            Assert.False(responseContent.Success);
            Assert.Equal(3, responseContent.Errors.Length);
            Assert.Null(responseContent.ApplicationError);
            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);

            Assert.Contains("O campo Nome é obrigatório.", responseContent.Errors);
            Assert.Contains("O campo Sobrenome é obrigatório.", responseContent.Errors);
            Assert.Contains("O campo Data de Nascimento é obrigatório.", responseContent.Errors);

            _repository.Verify(a => a.Adicionar(It.IsAny<Professor>()), Times.Never);
        }

        [Trait("ProfessorController", "Post_DadosInvalidos_BadRequest")]
        [Fact]
        public void Post_DadosInvalidos_BadRequest()
        {
            _professorController = new ProfessorController(_repository.Object, _validator, Mapper, _logger.Object);

            const string TEXTO_MAIOR_100_CARACTERES = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

            _professorModel.Nome = TEXTO_MAIOR_100_CARACTERES;
            _professorModel.Sobrenome = TEXTO_MAIOR_100_CARACTERES;
            _professorModel.DataNascimento = DateTime.Now;

            var response = _professorController.Post(_professorModel) as BadRequestObjectResult;
            var responseContent = response.Value as ApiResponse;

            Assert.False(responseContent.Success);
            Assert.Equal(3, responseContent.Errors.Length);
            Assert.Null(responseContent.ApplicationError);
            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);

            Assert.Contains("O campo Nome deve possuir no máximo 100 caracteres.", responseContent.Errors);
            Assert.Contains("O campo Sobrenome deve possuir no máximo 100 caracteres.", responseContent.Errors);
            Assert.Contains("A Data de Nascimento deve ser inferior a data atual.", responseContent.Errors);

            _repository.Verify(a => a.Adicionar(It.IsAny<Professor>()), Times.Never);
        }

        [Trait("ProfessorController", "Post_ErroNoProcessamento_InternalServerError")]
        [Fact]
        public void Post_ErroNoProcessamento_InternalServerError()
        {
            _repository.Setup(a => a.Adicionar(It.IsAny<Professor>())).Throws(new Exception("Falha ao persistir o registro na base."));

            _professorController = new ProfessorController(_repository.Object, _validator, Mapper, _logger.Object);

            var response = _professorController.Post(_professorModel) as ObjectResult;
            var responseContent = response.Value as ApiResponse;

            Assert.False(responseContent.Success);
            Assert.Null(responseContent.Errors);

            Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);

            Assert.Equal("Falha ao persistir o registro na base.", responseContent.ApplicationError);

            _repository.Verify(a => a.Adicionar(It.IsAny<Professor>()), Times.Once);
        }

        #endregion

        #region Atualizar professor - PUT

        [Trait("ProfessorController", "Put_DadosCorretos_AdicionarProfessorComSucesso")]
        [Fact]
        public void Put_DadosCorretos_AdicionarProfessorComSucesso()
        {
            _repository.Setup(a => a.BuscarPorId(It.IsAny<int>())).Returns(new Professor(1, "nome", "sobrenome", DateTime.Now.AddDays(-1)));
            _professorController = new ProfessorController(_repository.Object, _validator, Mapper, _logger.Object);

            var response = _professorController.Put(1, _professorModel) as OkObjectResult;
            var responseContent = response.Value as ApiResponse;

            Assert.True(responseContent.Success);
            Assert.Null(responseContent.Errors);
            Assert.Null(responseContent.ApplicationError);
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);

            _repository.Verify(a => a.BuscarPorId(It.IsAny<int>()), Times.Once);
            _repository.Verify(a => a.Atualizar(It.IsAny<Professor>()), Times.Once);
        }

        [Trait("ProfessorController", "Put_IdInformadoNaoEncontrado_NotFound")]
        [Fact]
        public void Put_IdInformadoNaoEncontrado_NotFound()
        {   
            _professorController = new ProfessorController(_repository.Object, _validator, Mapper, _logger.Object);

            var response = _professorController.Put(1, _professorModel) as NotFoundObjectResult;
            var responseContent = response.Value as ApiResponse;

            Assert.False(responseContent.Success);
            Assert.Single(responseContent.Errors);
            Assert.Null(responseContent.ApplicationError);
            Assert.Equal((int)HttpStatusCode.NotFound, response.StatusCode);

            Assert.Contains("Item não encontrado", responseContent.Errors);

            _repository.Verify(a => a.BuscarPorId(It.IsAny<int>()), Times.Once);
            _repository.Verify(a => a.Atualizar(It.IsAny<Professor>()), Times.Never);
        }

        [Trait("ProfessorController", "Put_CamposObrigatoriosNaoInformados_BadRequest")]
        [Fact]
        public void Put_CamposObrigatoriosNaoInformados_BadRequest()
        {
            _repository.Setup(a => a.BuscarPorId(_professorModel.Id)).Returns(new Professor(1, "nome", "sobrenome", DateTime.Now.AddDays(-1)));
            _professorController = new ProfessorController(_repository.Object, _validator, Mapper, _logger.Object);

            _professorModel.Nome = string.Empty;
            _professorModel.Sobrenome = string.Empty;
            _professorModel.DataNascimento = DateTime.MinValue;

            var response = _professorController.Put(1, _professorModel) as BadRequestObjectResult;
            var responseContent = response.Value as ApiResponse;

            Assert.False(responseContent.Success);
            Assert.Equal(3, responseContent.Errors.Length);
            Assert.Null(responseContent.ApplicationError);
            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);

            Assert.Contains("O campo Nome é obrigatório.", responseContent.Errors);
            Assert.Contains("O campo Sobrenome é obrigatório.", responseContent.Errors);
            Assert.Contains("O campo Data de Nascimento é obrigatório.", responseContent.Errors);

            _repository.Verify(a => a.BuscarPorId(It.IsAny<int>()), Times.Once);
            _repository.Verify(a => a.Atualizar(It.IsAny<Professor>()), Times.Never);
        }

        [Trait("ProfessorController", "Put_DadosInvalidos_BadRequest")]
        [Fact]
        public void Put_DadosInvalidos_BadRequest()
        {
            _repository.Setup(a => a.BuscarPorId(_professorModel.Id)).Returns(new Professor(1, "nome", "sobrenome", DateTime.Now.AddDays(-1)));
            _professorController = new ProfessorController(_repository.Object, _validator, Mapper, _logger.Object);

            const string TEXTO_MAIOR_100_CARACTERES = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

            _professorModel.Nome = TEXTO_MAIOR_100_CARACTERES;
            _professorModel.Sobrenome = TEXTO_MAIOR_100_CARACTERES;
            _professorModel.DataNascimento = DateTime.Now;

            var response = _professorController.Put(1, _professorModel) as BadRequestObjectResult;
            var responseContent = response.Value as ApiResponse;

            Assert.False(responseContent.Success);
            Assert.Equal(3, responseContent.Errors.Length);
            Assert.Null(responseContent.ApplicationError);
            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);

            Assert.Contains("O campo Nome deve possuir no máximo 100 caracteres.", responseContent.Errors);
            Assert.Contains("O campo Sobrenome deve possuir no máximo 100 caracteres.", responseContent.Errors);
            Assert.Contains("A Data de Nascimento deve ser inferior a data atual.", responseContent.Errors);

            _repository.Verify(a => a.BuscarPorId(It.IsAny<int>()), Times.Once);
            _repository.Verify(a => a.Atualizar(It.IsAny<Professor>()), Times.Never);
        }

        [Trait("ProfessorController", "Put_ErroNoProcessamento_InternalServerError")]
        [Fact]
        public void Put_ErroNoProcessamento_InternalServerError()
        {
            _repository.Setup(a => a.BuscarPorId(_professorModel.Id)).Returns(new Professor(1, "nome", "sobrenome", DateTime.Now.AddDays(-1)));
            _repository.Setup(a => a.Atualizar(It.IsAny<Professor>())).Throws(new Exception("Falha ao persistir o registro na base."));

            _professorController = new ProfessorController(_repository.Object, _validator, Mapper, _logger.Object);

            var response = _professorController.Put(1, _professorModel) as ObjectResult;
            var responseContent = response.Value as ApiResponse;

            Assert.False(responseContent.Success);
            Assert.Null(responseContent.Errors);

            Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);

            Assert.Equal("Falha ao persistir o registro na base.", responseContent.ApplicationError);

            _repository.Verify(a => a.Atualizar(It.IsAny<Professor>()), Times.Once);
            _repository.Verify(a => a.BuscarPorId(It.IsAny<int>()), Times.Once);
        }

        #endregion
    }
}
