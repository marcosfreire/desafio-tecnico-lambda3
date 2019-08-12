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
    public class DisciplinaController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ValidadorDisciplina _validator;
        private readonly ApplicationDataContext _applicationDataContext;
        private readonly ILogger<DisciplinaController> _logger;

        public DisciplinaController(IMapper mapper,
                                    ValidadorDisciplina validator, 
                                    ApplicationDataContext applicationDataContext, 
                                    ILogger<DisciplinaController> logger)
        {
            _mapper = mapper;
            _validator = validator;
            _applicationDataContext = applicationDataContext;
            _logger = logger;
        }

        [HttpGet]
        [Route("disciplinas")]
        public IActionResult Get()
        {
            var disciplinas = _mapper.Map<IEnumerable<Disciplina>, IEnumerable<DisciplinaModel>>(_applicationDataContext.Disciplinas.ToList());
            return Ok(disciplinas);
        }

        [HttpPost]
        [Route("disciplinas")]
        public IActionResult Post(DisciplinaModel model)
        {
            try
            {
                var disciplina = _mapper.Map<DisciplinaModel, Disciplina>(model);

                if (!_validator.Valido(disciplina))
                    return BadRequest(new ApiResponse { Data = model, Errors = _validator.ErrorMessages });

                _applicationDataContext.Disciplinas.Add(disciplina);
                _applicationDataContext.SaveChanges();

                model.Id = disciplina.Id;

                return Ok(new ApiResponse { Data = model });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse { Data = model, ApplicationError = ex.Message });
            }
        }

        [HttpDelete]
        [Route("disciplinas/{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var disciplinaDb = _applicationDataContext.Disciplinas.FirstOrDefault(a => a.Id == id);

                if (disciplinaDb == null)
                {
                    _validator.ValidationResult.Errors.Add(new ValidationFailure("", "Item não encontrado"));
                    return NotFound(new ApiResponse { Errors = _validator.ErrorMessages });
                }

                _applicationDataContext.Disciplinas.Remove(disciplinaDb);
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