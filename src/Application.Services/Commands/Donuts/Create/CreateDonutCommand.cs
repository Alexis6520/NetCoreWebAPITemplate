using Application.Services.Abstractions;

namespace Application.Services.Commands.Donuts.Create
{
    /// <summary>
    /// Comando para crear una dona
    /// </summary>
    /// <param name="name">Nombre de la dona</param>
    /// <param name="price">Precio de la dona</param>
    public class CreateDonutCommand(string name, decimal price) : IRequest<int>
    {
        /// <summary>
        /// Nombre de la dona
        /// </summary>
        public string Name { get; set; } = name;

        /// <summary>
        /// Precio de la dona
        /// </summary>
        public decimal Price { get; set; } = price;

        /// <summary>
        /// Descripción de la dona
        /// </summary>
        public string? Description { get; set; }
    }
}
