using MediatR;
using Microsoft.Extensions.Logging;
using Services;
using Services.Wrappers;
using System.Net;
using System.Text.Json.Serialization;

namespace Logic.Handlers.DonutHandlers
{
    /// <summary>
    /// Comando para actualizar una dona
    /// </summary>
    public class DonutUpdateCommand : IRequest<Result>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class DonutUpdateHandler(IUnitOfWork unitOfWork, ILogger<DonutUpdateHandler> logger) : IRequestHandler<DonutUpdateCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DonutUpdateHandler> _logger = logger;

        public async Task<Result> Handle(DonutUpdateCommand request, CancellationToken cancellationToken)
        {
            var donut = await _unitOfWork.Donuts.FindAsync([request.Id], cancellationToken);

            if (donut == null)
            {
                return Result.Failed(HttpStatusCode.BadRequest, ["No existe la dona especificada"]);
            }

            donut.Name = request.Name;
            donut.Price = request.Price;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Dona {id} actualizada", donut.Id);
            return Result.Success(HttpStatusCode.NoContent);
        }
    }
}
