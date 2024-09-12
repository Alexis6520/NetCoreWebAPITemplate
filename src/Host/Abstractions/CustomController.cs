using Application.Services.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Host.Abstractions
{
    /// <summary>
    /// Abstracción de controlador personalizado
    /// </summary>
    /// <param name="mediator">Mediador de manejadores</param>
    [Route("[controller]")]
    [ApiController]
    public abstract class CustomController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Mediador de manejo de solicitudes
        /// </summary>
        protected readonly IMediator Mediator = mediator;

        /// <summary>
        /// Devuelve una respuesta sin contenido
        /// </summary>
        /// <param name="response">Respuesta a devolver</param>
        /// <returns></returns>
        protected ObjectResult CustomResponse(Response response)
        {
            var statusCode = (int)response.StatusCode;
            if (!response.Succeeded) return StatusCode(statusCode, response);
            return StatusCode(statusCode, null);
        }

        /// <summary>
        /// Devuelve una respuesta con contenido
        /// </summary>
        /// <typeparam name="T">Tipo de dato a devolver</typeparam>
        /// <param name="response">Respuesta a devolver</param>
        /// <returns></returns>
        protected ObjectResult CustomResponse<T>(Response<T> response)
        {
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
