using Application.Services.Commands.Donuts.Create;
using Application.Services.Wrappers;
using Host.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    public class DonutsController(IMediator mediator) : CustomController(mediator)
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Response<int>>> Create(CreateDonutCommand command)
        {
            return CustomResponse(await Mediator.Send(command));
        }
    }
}
