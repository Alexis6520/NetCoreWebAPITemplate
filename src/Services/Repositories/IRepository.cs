using System.Linq.Expressions;

namespace Services.Repositories
{
    /// <summary>
    /// Repositorio genérico
    /// </summary>
    /// <typeparam name="TEntity">Tipo de la entidad</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Agrega una entidad al contexto
        /// </summary>
        /// <param name="entity">Entidad a agregar</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Busca una entidad por sus identificadores
        /// </summary>
        /// <param name="keys">Identificadores</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity?> FindAsync(object[] keys, CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene todas las entidades que coinciden con el filtro proporcionado
        /// </summary>
        /// <param name="filter">Filtro</param>
        /// <param name="includeProperties">Propiedades a incluir en la consulta</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null, string includeProperties = "", CancellationToken cancellationToken = default);

        /// <summary>
        /// Elimina una entidad del contexto
        /// </summary>
        /// <param name="entity"></param>
        void Remove(TEntity entity);
    }
}
