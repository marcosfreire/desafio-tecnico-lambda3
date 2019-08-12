using AutoMapper;
using DesafioTecnico.Lambda3.Api.Models;
using DesafioTecnico.Lamda3.Domain;
using DesafioTecnico.Lamda3.Repository.Context;
using FluentValidation.Results;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ApplicationDataContext _applicationDataContext;
        private readonly ILogger<ProfessorController> _logger;

        public ProfessorController(ApplicationDataContext applicationDataContext,
                                   ValidadorProfessor validator,
                                   IMapper mapper,
                                   ILogger<ProfessorController> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _validator = validator;
            _applicationDataContext = applicationDataContext;
        }

        [HttpGet]
        [Route("professores")]
        public IActionResult Get()
        {
            var professores = _mapper.Map<IEnumerable<Professor>, IEnumerable<ProfessorModel>>(_applicationDataContext.Professores.ToList());
            return Ok(professores);
        }

        [HttpGet]
        [Route("professores/{id:int}")]
        public IActionResult Get(int id)
        {
            var professor = _mapper.Map<Professor, ProfessorModel>(_applicationDataContext.Professores.FirstOrDefault(a => a.Id == id));
            return Ok(professor);
        }

        [HttpPost]
        [Route("professores")]
        public IActionResult Post(ProfessorModel model)
        {
            try
            {
                var professor = _mapper.Map<ProfessorModel, Professor>(model);

                if (!_validator.Valido(professor))
                    return BadRequest(new ApiResponse { Data = model, Errors = _validator.ErrorMessages });

                _applicationDataContext.Professores.Add(professor);
                _applicationDataContext.SaveChanges();

                model.Id = professor.Id;

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
                var professorDb = _applicationDataContext.Professores.FirstOrDefault(a => a.Id == id);

                if (professorDb == null)
                {
                    _validator.ValidationResult.Errors.Add(new ValidationFailure("", "Item não encontrado"));
                    return NotFound(new ApiResponse { Data = model, Errors = _validator.ErrorMessages });
                }

                var professor = _mapper.Map<ProfessorModel, Professor>(model);

                if (!_validator.Valido(professor))
                    return BadRequest(new ApiResponse { Data = model, Errors = _validator.ErrorMessages });

                professorDb.AtualizarDados(professor);

                _applicationDataContext.Professores.Update(professorDb);
                _applicationDataContext.SaveChanges();

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
                var professorDb = _applicationDataContext.Professores.FirstOrDefault(a => a.Id == id);

                if (professorDb == null)
                {
                    _validator.ValidationResult.Errors.Add(new ValidationFailure("", "Item não encontrado"));
                    return NotFound(new ApiResponse { Errors = _validator.ErrorMessages });
                }

                _applicationDataContext.Professores.Remove(professorDb);
                _applicationDataContext.SaveChanges();

                return Ok(new ApiResponse { Data = "" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse { ApplicationError = ex.Message });
            }
        }
    }
}
