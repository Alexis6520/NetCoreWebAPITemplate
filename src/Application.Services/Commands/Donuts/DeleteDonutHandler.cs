using Application.Services.Abstractions;
using Application.Services.Wrappers;
using Domain.Entities;
using Domain.Services;
using System.Net;

namespace Application.Services.Commands.Donuts
{
    public class DeleteDonutHandler(ApplicationDbContext dbContext) : IRequestHandler<DeleteCommand<int, Donut>>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<Response> Handle(DeleteCommand<int, Donut> request, CancellationToken cancellationToken)
        {
            var donut = await _dbContext.Donuts
                .FindAsync([request.Id], cancellationToken);

            if (donut == null) return Response.Fail(HttpStatusCode.NotFound, "Dona no encontrada");
            _dbContext.Donuts.Remove(donut);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Response.Success();
        }
    }
}
