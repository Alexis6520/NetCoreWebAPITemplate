using Application.Services.Abstractions;
using Application.Services.DTOs.Donuts;
using Application.Services.Wrappers;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Application.Services.Queries.Donuts
{
    public class DonutQueryHandler(ApplicationDbContext dbContext) :
        IRequestHandler<GetDonutListQuery, List<DonutListItemDTO>>,
        IRequestHandler<FindQuery<int, DonutDTO>, DonutDTO>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<Response<List<DonutListItemDTO>>> Handle(GetDonutListQuery request, CancellationToken cancellationToken)
        {
            var list = await _dbContext.Donuts
                .Select(x => new DonutListItemDTO
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .OrderBy(x => x.Id)
                .ToListAsync(cancellationToken);

            return Response<List<DonutListItemDTO>>.Success(list);
        }

        public async Task<Response<DonutDTO>> Handle(FindQuery<int, DonutDTO> request, CancellationToken cancellationToken)
        {
            var donut = await _dbContext.Donuts
                .Where(x => x.Id == request.Id)
                .Select(x => new DonutDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (donut == null) return Response<DonutDTO>.Fail(HttpStatusCode.NotFound, "Dona no encontrada");
            return Response<DonutDTO>.Success(donut);
        }
    }
}
