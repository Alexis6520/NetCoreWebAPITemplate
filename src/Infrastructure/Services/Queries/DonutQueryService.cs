using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Persistence;
using Services.Queries;
using Services.Queries.DTOs.DonutDTOs;
using Services.Wrappers;
using System.Net;

namespace Infrastructure.Services.Queries
{
    public class DonutQueryService(ApplicationDbContext dbContext, IMapper mapper) : IDonutQueryService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<PaginatedList<DonutDTO>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Donuts
                .ProjectTo<DonutDTO>(_mapper.ConfigurationProvider)
                .OrderBy(x => x.Id)
                .GetPaginatedAsync(page, pageSize, cancellationToken);
        }

        public Result<DonutDTO> GetById(int id)
        {
            var donut = _dbContext.Donuts
                .Where(x => x.Id == id)
                .ProjectTo<DonutDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefault();

            if (donut == null)
            {
                return Result<DonutDTO>.Failed(HttpStatusCode.NotFound, ["Dona no encontrada"]);
            }

            return Result<DonutDTO>.Success(donut);
        }
    }
}
