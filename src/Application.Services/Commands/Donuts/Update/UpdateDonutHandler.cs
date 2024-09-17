using Application.Services.Abstractions;
using Application.Services.Wrappers;
using Domain.Services;
using System.Net;

namespace Application.Services.Commands.Donuts.Update
{
    public class UpdateDonutHandler(ApplicationDbContext dbContext) : IRequestHandler<UpdateDonutCommand>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<Response> Handle(UpdateDonutCommand request, CancellationToken cancellationToken)
        {
            var donut = await _dbContext.Donuts
                .FindAsync([request.Id], cancellationToken);

            if (donut == null) return Response.Fail(HttpStatusCode.NotFound, "Dona no encontrada");
            donut.Name = request.Name;
            donut.Description = request.Description;
            donut.Price = request.Price;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Response.Success();
        }
    }
}
