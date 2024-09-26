using Application.Services.Abstractions;
using Application.Services.Wrappers;
using Domain.Entities;
using Domain.Services;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.Services.Commands.Donuts
{
    public class DeleteDonutHandler(ApplicationDbContext dbContext, ILogger<DeleteDonutHandler> logger) : IRequestHandler<DeleteCommand<int, Donut>>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly ILogger<DeleteDonutHandler> _logger = logger;

        public async Task<Response> Handle(DeleteCommand<int, Donut> request, CancellationToken cancellationToken)
        {
            var donut = await _dbContext.Donuts
                .FindAsync([request.Id], cancellationToken);

            if (donut == null) return Response.Fail(HttpStatusCode.NotFound, "Dona no encontrada");
            _dbContext.Donuts.Remove(donut);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Dona {Id} eliminada", donut.Id);
            return Response.Success();
        }
    }
}
