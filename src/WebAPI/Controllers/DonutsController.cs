using Logic.Handlers.DonutHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class DonutsController(IMediator mediator) : CustomController
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Crea una dona
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAsync(DonutCreateCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CustomResult(result);
        }
    }
}
