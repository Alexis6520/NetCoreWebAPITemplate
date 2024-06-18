using MediatR;
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

    public class DonutDeleteHandler(IUnitOfWork unitOfWork) : IRequestHandler<DonutDeleteCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(DonutDeleteCommand request, CancellationToken cancellationToken)
        {
            var donut = await _unitOfWork.Donuts.FindAsync([request.Id], cancellationToken);
            if (donut == null) return Result.Failed(HttpStatusCode.BadRequest, ["La dona especificada no existe"]);
            _unitOfWork.Donuts.Remove(donut);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success(HttpStatusCode.NoContent);
        }
    }
}
