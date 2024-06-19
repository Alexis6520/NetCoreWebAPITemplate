using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Services;
using Services.Wrappers;
using System.Net;

namespace Logic.Handlers.DonutHandlers
{
    /// <summary>
    /// Comando para crear una dona
    /// </summary>
    /// <param name="name"></param>
    /// <param name="price"></param>
    public class DonutCreateCommand(string name, decimal price) : IRequest<Result>
    {
        public string Name { get; set; } = name;
        public decimal Price { get; set; } = price;
    }

    public class DonutCreateHandler(IUnitOfWork unitOfWork, ILogger<DonutCreateHandler> logger) : IRequestHandler<DonutCreateCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DonutCreateHandler> _logger = logger;

        public async Task<Result> Handle(DonutCreateCommand request, CancellationToken cancellationToken)
        {
            throw new Exception("Error de prueba");
            var donut = new Donut(request.Name, request.Price);
            await _unitOfWork.Donuts.AddAsync(donut, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Nueva dona {id}", donut.Id);
            return Result.Success(HttpStatusCode.Created);
        }
    }
}
