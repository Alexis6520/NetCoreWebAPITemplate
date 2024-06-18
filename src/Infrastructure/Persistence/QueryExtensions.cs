using Microsoft.EntityFrameworkCore;
using Services.Wrappers;

namespace Infrastructure.Persistence
{
    public static class QueryExtensions
    {
        /// <summary>
        /// Obtiene una página de resultados de una consulta
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="page">Página</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<PaginatedList<T>> GetPaginatedAsync<T>(this IQueryable<T> query, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var totalCount = await query.CountAsync(cancellationToken);
            if (page < 1) throw new ArgumentException("El valor debe ser mayor a 0", nameof(page));
            if (pageSize < 1) throw new ArgumentException("El valor debe ser mayor a 0", nameof(pageSize));

            return new PaginatedList<T>
            {
                Page = page,
                PageCount = (int)Math.Ceiling(totalCount / (decimal)pageSize),
                TotalCount = totalCount,
                Items = await query.Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken)
            };
        }
    }
}
