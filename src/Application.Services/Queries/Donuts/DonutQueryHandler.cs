using Application.Services.Abstractions;
using Application.Services.DTOs.Donuts;
using Application.Services.Wrappers;
using Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Queries.Donuts
{
    public class DonutQueryHandler(ApplicationDbContext dbContext) : IRequestHandler<GetDonutListQuery, List<DonutDTO>>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<Response<List<DonutDTO>>> Handle(GetDonutListQuery request, CancellationToken cancellationToken)
        {
            var list = await _dbContext.Donuts
                .Select(x => new DonutDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                })
                .OrderBy(x => x.Id)
                .ToListAsync(cancellationToken);

            return Response<List<DonutDTO>>.Success(list);
        }
    }
}
