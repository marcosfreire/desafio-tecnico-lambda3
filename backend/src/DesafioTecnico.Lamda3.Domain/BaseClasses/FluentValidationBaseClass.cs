using FluentValidation;
using FluentValidation.Results;
using System.Linq;

namespace DesafioTecnico.Lamda3.Domain.BaseClasses
{
    public abstract class FluentValidationBaseClass<T> : AbstractValidator<T> where T : class
    {
        public ValidationResult ValidationResult { get; set; }
        public string[] ErrorMessages => ValidationResult?.Errors?.Select(a => a.ErrorMessage)?.ToArray();
    }
}
