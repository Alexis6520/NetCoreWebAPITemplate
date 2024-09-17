using Application.Services.Abstractions;
using Application.Services.Wrappers;
using Domain.Entities;
using Domain.Services;
using System.Net;

namespace Application.Services.Commands.Donuts.Create
{
    public class CreateDonutHandler(ApplicationDbContext dbContext) : IRequestHandler<CreateDonutCommand, int>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<Response<int>> Handle(CreateDonutCommand request, CancellationToken cancellationToken)
        {
            var donut = new Donut(request.Name, request.Price)
            {
                Description = request.Description,
            };

            await _dbContext.Donuts.AddAsync(donut, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Response<int>.Success(donut.Id, HttpStatusCode.Created);
        }
    }
}
