using Application.Services.Commands;
using Application.Services.Commands.Donuts.Create;
using Application.Services.Commands.Donuts.Update;
using Application.Services.DTOs.Donuts;
using Application.Services.Queries;
using Application.Services.Queries.Donuts;
using Application.Services.Wrappers;
using Domain.Entities;
using Host.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    public class DonutsController(IMediator mediator) : CustomController(mediator)
    {
        /// <summary>
        /// Crea una dona
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Response<int>>> Create(CreateDonutCommand command)
        {
            return CustomResponse(await Mediator.Send(command));
        }

        /// <summary>
        /// Lista de donas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Response<List<DonutListItemDTO>>>> GetAll()
        {
            return CustomResponse(await Mediator.Send(new GetDonutListQuery()));
        }

        /// <summary>
        /// Obtiene una dona
        /// </summary>
        /// <param name="id">Id de la dona</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<DonutDTO>>> Get(int id)
        {
            return CustomResponse(await Mediator.Send(new FindQuery<int, DonutDTO>(id)));
        }

        /// <summary>
        /// Actualiza una dona
        /// </summary>
        /// <param name="id">Id de la dona</param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, UpdateDonutCommand command)
        {
            command.Id = id;
            return CustomResponse(await Mediator.Send(command));
        }

        /// <summary>
        /// Elimina una dona
        /// </summary>
        /// <param name="id">Id de la dona</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Response<DonutDTO>>> Delete(int id)
        {
            return CustomResponse(await Mediator.Send(new DeleteCommand<int, Donut>(id)));
        }
    }
}
