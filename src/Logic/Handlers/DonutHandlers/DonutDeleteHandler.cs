using MediatR;
using Microsoft.Extensions.Logging;
using Services;
using Services.Wrappers;
using System.Net;

namespace Logic.Handlers.DonutHandlers
{
    /// <summary>
    /// Comando para eliminar dona
    /// </summary>
    /// <param name="id"></param>
    public class DonutDeleteCommand(int id) : IRequest<Result>
    {
        public int Id { get; set; } = id;
    }

    public class DonutDeleteHandler(IUnitOfWork unitOfWork, ILogger<DonutDeleteHandler> logger) : IRequestHandler<DonutDeleteCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DonutDeleteHandler> _logger = logger;

        public async Task<Result> Handle(DonutDeleteCommand request, CancellationToken cancellationToken)
        {
            var donut = await _unitOfWork.Donuts.FindAsync([request.Id], cancellationToken);
            if (donut == null) return Result.Failed(HttpStatusCode.BadRequest, ["La dona especificada no existe"]);
            _unitOfWork.Donuts.Remove(donut);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Dona {id} eliminada", donut.Id);
            return Result.Success(HttpStatusCode.NoContent);
        }
    }
}
