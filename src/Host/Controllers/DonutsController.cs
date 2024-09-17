using Application.Services.Commands.Donuts.Create;
using Application.Services.DTOs.Donuts;
using Application.Services.Queries.Donuts;
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

        [HttpGet]
        public async Task<ActionResult<Response<List<DonutDTO>>>> GetAll()
        {
            return CustomResponse(await Mediator.Send(new GetDonutListQuery()));
        }
    }
}
