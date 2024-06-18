using MediatR;
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
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
    }

    public class DonutUpdateHandler(IUnitOfWork unitOfWork) : IRequestHandler<DonutUpdateCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

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
            return Result.Success(HttpStatusCode.NoContent);
        }
    }
}
