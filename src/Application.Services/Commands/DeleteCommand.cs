using Application.Services.Abstractions;

namespace Application.Services.Commands
{
    /// <summary>
    /// Comando genérico para eliminar un recurso usando un Id
    /// </summary>
    /// <typeparam name="I">Tipo de Id</typeparam>
    /// <typeparam name="R">Tipo de recurso a eliminar</typeparam>
    /// <param name="id">Id de recurso a eliminar</param>
    public class DeleteCommand<I, R>(int id) : IRequest where R : class
    {
        public int Id { get; set; } = id;
    }
}
