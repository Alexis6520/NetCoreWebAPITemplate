using Services.Queries.DTOs.DonutDTOs;
using Services.Wrappers;

namespace Services.Queries
{
    /// <summary>
    /// Servicio de consulta de donas
    /// </summary>
    public interface IDonutQueryService
    {
        /// <summary>
        /// Obtiene una dona por Id
        /// </summary>
        /// <param name="id">Id de la dona</param>
        /// <returns></returns>
        Result<DonutDTO> GetById(int id);

        /// <summary>
        /// Obtiene todas las donas de forma paginada
        /// </summary>
        /// <param name="page">Pagina a consultar</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PaginatedList<DonutDTO>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    }
}
