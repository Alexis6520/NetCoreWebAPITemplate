using Domain.Entities;
using Services.Repositories;

namespace Services
{
    /// <summary>
    /// Unidad de trabajo de acceso a la persistencia para servicios de lógica de negocio
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Donas
        /// </summary>
        IRepository<Donut> Donuts { get; }

        /// <summary>
        /// Guarda los cambios realizados a las entidades y el contexto de la base de datos
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
