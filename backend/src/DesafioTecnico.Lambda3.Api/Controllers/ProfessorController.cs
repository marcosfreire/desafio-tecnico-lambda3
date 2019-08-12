using AutoMapper;
using DesafioTecnico.Lambda3.Api.Models;
using DesafioTecnico.Lamda3.Domain;
using DesafioTecnico.Lamda3.Repository;
using FluentValidation.Results;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;

namespace DesafioTecnico.Lambda3.Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [EnableCors("*")]
    public class ProfessorController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ValidadorProfessor _validator;
        private readonly IProfessorRepository _professorRepository;
        private readonly ILogger<ProfessorController> _logger;

        public ProfessorController(IProfessorRepository repository,
                                   ValidadorProfessor validator,
                                   IMapper mapper,
                                   ILogger<ProfessorController> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _validator = validator;
            _professorRepository = repository;
        }

        [HttpGet]
        [Route("professores")]
        public IActionResult Get()
        {
            var professores = _mapper.Map<IEnumerable<Professor>, IEnumerable<ProfessorModel>>(_professorRepository.BuscarTodos());
            return Ok(professores);
        }

        [HttpGet]
        [Route("professores/{id:int}")]
        public IActionResult Get(int id)
        {
            var professor = _mapper.Map<Professor, ProfessorModel>(_professorRepository.BuscarPorId(id));
            return Ok(professor);
        }

        [HttpPost]
        [Route("professores")]
        public IActionResult Post(ProfessorModel model)
        {
            try
            {
                var professor = _mapper.Map<ProfessorModel, Professor>(model);

                if (!ProfessorValido(professor))
                    return BadRequest(new ApiResponse { Data = model, Errors = _validator.ErrorMessages });

                Adicionar(model, professor);

                return Ok(new ApiResponse { Data = model });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse { Data = model, ApplicationError = ex.Message });
            }
        }

        [HttpPut]
        [Route("professores/{id:int}")]
        public IActionResult Put(int id, ProfessorModel model)
        {
            try
            {
                var professorDb = _professorRepository.BuscarPorId(id);

                if (RegistroNaoEncontrado(professorDb))
                {
                    _validator.ValidationResult.Errors.Add(new ValidationFailure("", "Item não encontrado"));
                    return NotFound(new ApiResponse { Data = model, Errors = _validator.ErrorMessages });
                }

                var professor = _mapper.Map<ProfessorModel, Professor>(model);

                if (!ProfessorValido(professor))
                    return BadRequest(new ApiResponse { Data = model, Errors = _validator.ErrorMessages });

                AtualizarProfessor(professorDb, professor);

                return Ok(new ApiResponse { Data = model });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse { Data = model, ApplicationError = ex.Message });
            }
        }

        [HttpDelete]
        [Route("professores/{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var professor = _professorRepository.BuscarPorId(id);

                if (RegistroNaoEncontrado(professor))
                {
                    _validator.ValidationResult.Errors.Add(new ValidationFailure("", "Item não encontrado"));
                    return NotFound(new ApiResponse { Errors = _validator.ErrorMessages });
                }

                RemoverProfessor(professor);

                return Ok(new ApiResponse { Data = "" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse { ApplicationError = ex.Message });
            }
        }

        private void RemoverProfessor(Professor professor)
        {
            _professorRepository.Remover(professor);
        }

        private void AtualizarProfessor(Professor professorDb, Professor professor)
        {
            professorDb.AtualizarDados(professor);
            _professorRepository.Atualizar(professorDb);
        }

        private bool ProfessorValido(Professor professor)
        {
            return _validator.Valido(professor);
        }

        private bool RegistroNaoEncontrado(Professor professor)
        {
            return professor == null;
        }

        private void Adicionar(ProfessorModel model, Professor professor)
        {
            _professorRepository.Adicionar(professor);
            model.Id = professor.Id;

        }
    }
}
