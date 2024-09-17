using Application.Services.Abstractions;

namespace Application.Services.Queries
{
    /// <summary>
    /// Consulta genérica que usa un Id o referencia
    /// </summary>
    /// <typeparam name="I">Tipo de Id/Referencia</typeparam>
    /// <typeparam name="V">Tipo de valor a devolver</typeparam>
    /// <typeparam name="V">Tipo de valor a devolver</typeparam>
    /// <param name="id">Id/Referencia</param>
    public class FindQuery<I, V>(I id) : IRequest<V>
    {
        public I Id { get; set; } = id;
    }
}
