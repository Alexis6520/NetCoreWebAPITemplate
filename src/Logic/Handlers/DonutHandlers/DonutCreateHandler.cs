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
    public class DonutCreateCommand(string name, decimal price) : IRequest<Result<int>>
    {
        public string Name { get; set; } = name;
        public decimal Price { get; set; } = price;
    }

    public class DonutCreateHandler(IUnitOfWork unitOfWork, ILogger<DonutCreateHandler> logger) : IRequestHandler<DonutCreateCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DonutCreateHandler> _logger = logger;

        public async Task<Result<int>> Handle(DonutCreateCommand request, CancellationToken cancellationToken)
        {
            var donut = new Donut(request.Name, request.Price);
            await _unitOfWork.Donuts.AddAsync(donut, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Nueva dona {id}", donut.Id);
            return Result<int>.Success(donut.Id, HttpStatusCode.Created);
        }
    }
}
