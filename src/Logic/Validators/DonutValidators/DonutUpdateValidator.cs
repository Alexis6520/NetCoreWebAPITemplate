using FluentValidation;
using Logic.Handlers.DonutHandlers;

namespace Logic.Validators.DonutValidators
{
    public class DonutUpdateValidator : AbstractValidator<DonutUpdateCommand>
    {
        public DonutUpdateValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(50).WithMessage("El nombre no puede superar los {MaxLength} caracteres");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("No se admiten precios negativos");
        }
    }
}
