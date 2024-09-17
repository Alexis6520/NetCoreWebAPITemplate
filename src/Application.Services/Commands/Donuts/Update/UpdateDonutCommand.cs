using Application.Services.Abstractions;
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
        public string Name { get; set; } = "";

        /// <summary>
        /// Nueva descripción
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Nuevo precio
        /// </summary>
        public decimal Price { get; set; }
    }
}
