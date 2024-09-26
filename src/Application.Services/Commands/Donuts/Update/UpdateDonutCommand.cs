using Application.Services.Abstractions;
using FluentValidation;
using System.Text.Json.Serialization;

namespace Application.Services.Commands.Donuts.Update
{
    /// <summary>
    /// Comando para actualizar una dona
    /// </summary>
    public class UpdateDonutCommand : IRequest
    {
        /// <summary>
        /// Id de dona
        /// </summary>
        [JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// Nuevo nombre
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Nueva descripción
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Nuevo precio
        /// </summary>
        public decimal Price { get; set; }
    }

    public class UpdateDonutValidator : AbstractValidator<UpdateDonutCommand>
    {
        public UpdateDonutValidator() 
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
