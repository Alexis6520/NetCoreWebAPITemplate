namespace Domain.Entities
{
    /// <summary>
    /// Representa una dona del Krispy Kreme
    /// </summary>
    /// <param name="name">Nombre de la dona</param>
    /// <param name="price">Precio de la dona</param>
    public class Donut(string name, decimal price)
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la dona
        /// </summary>
        public string Name { get; set; } = name;

        /// <summary>
        /// Descripción de la dona
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Precio de la dona
        /// </summary>
        public decimal Price { get; set; } = price;
    }
}
