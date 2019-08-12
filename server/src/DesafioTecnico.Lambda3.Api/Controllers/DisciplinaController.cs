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
    public class DisciplinaController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ValidadorDisciplina _validator;
        private readonly IDisciplinaRepository _disciplinaRepository;

        private readonly ILogger<DisciplinaController> _logger;

        public DisciplinaController(IMapper mapper,
                                    ValidadorDisciplina validator,
                                    IDisciplinaRepository repository, 
                                    ILogger<DisciplinaController> logger)
        {
            _mapper = mapper;
            _validator = validator;
            _disciplinaRepository = repository;
            _logger = logger;
        }

        [HttpGet]
        [Route("disciplinas")]
        public IActionResult Get()
        {
            var disciplinas = _mapper.Map<IEnumerable<Disciplina>, IEnumerable<DisciplinaModel>>(_disciplinaRepository.BuscarTodos());
            return Ok(disciplinas);
        }

        [HttpPost]
        [Route("disciplinas")]
        public IActionResult Post(DisciplinaModel model)
        {
            try
            {
                var disciplina = _mapper.Map<DisciplinaModel, Disciplina>(model);

                if (!DisciplinaValida(disciplina))
                    return BadRequest(new ApiResponse { Data = model, Errors = _validator.ErrorMessages });

                AdicionarDisciplina(model, disciplina);

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
                var disciplina = _disciplinaRepository.BuscarPorId(id);

                if (disciplina == null)
                {
                    _validator.ValidationResult.Errors.Add(new ValidationFailure("", "Item não encontrado"));
                    return NotFound(new ApiResponse { Errors = _validator.ErrorMessages });
                }

                _disciplinaRepository.Remover(disciplina);

                return Ok(new ApiResponse { Data = "" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse { ApplicationError = ex.Message });
            }
        }

        private void AdicionarDisciplina(DisciplinaModel model, Disciplina disciplina)
        {
            _disciplinaRepository.Adicionar(disciplina);
            model.Id = disciplina.Id;
        }

        private bool DisciplinaValida(Disciplina disciplina)
        {
            return _validator.Valido(disciplina);
        }
    }
}