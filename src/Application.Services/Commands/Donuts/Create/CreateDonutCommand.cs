using Application.Services.Abstractions;
using FluentValidation;

namespace Application.Services.Commands.Donuts.Create
{
    /// <summary>
    /// Comando para crear una dona
    /// </summary>
    public class CreateDonutCommand : IRequest<int>
    {
        /// <summary>
        /// Nombre de la dona
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Precio de la dona
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Descripción de la dona
        /// </summary>
        public string Description { get; set; }
    }

    public class CreateDonutValidator : AbstractValidator<CreateDonutCommand>
    {
        public CreateDonutValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(30).WithMessage("El nombre no puede superar los {MaxLength} caracteres");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("No se admiten valores negativos");

            RuleFor(x => x.Description)
                .MaximumLength(512).WithMessage("La descripción no puede superar los {MaxLength} caracteres");
        }
    }
}
