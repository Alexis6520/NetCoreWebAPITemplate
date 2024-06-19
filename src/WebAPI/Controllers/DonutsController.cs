using Logic.Handlers.DonutHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Queries;
using Services.Queries.DTOs.DonutDTOs;
using Services.Wrappers;

namespace WebAPI.Controllers
{
    public class DonutsController(IMediator mediator, IDonutQueryService queryService) : CustomController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IDonutQueryService _queryService = queryService;

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

        /// <summary>
        /// Obtiene todas las donas de forma paginada
        /// </summary>
        /// <param name="page">Página a consultar</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PaginatedList<DonutDTO>>> GetAllAsync(int page = 1, int pageSize = 5, CancellationToken cancellationToken = default)
        {
            var donuts = await _queryService.GetAllAsync(page, pageSize, cancellationToken);
            return Ok(donuts);
        }

        /// <summary>
        /// Obtiene una dona por Id
        /// </summary>
        /// <param name="id">Id de la dona</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Result<DonutDTO>> GetById(int id)
        {
            var result = _queryService.GetById(id);
            return CustomResult(result);
        }

        /// <summary>
        /// Actualiza una dona
        /// </summary>
        /// <param name="id">Id de dona</param>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateAsync(int id, DonutUpdateCommand command, CancellationToken cancellationToken = default)
        {
            command.Id = id;
            var result = await _mediator.Send(command, cancellationToken);
            return CustomResult(result);
        }

        /// <summary>
        /// Elimina una dona
        /// </summary>
        /// <param name="id">Id de dona</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new DonutDeleteCommand(id), cancellationToken);
            return CustomResult(result);
        }
    }
}
