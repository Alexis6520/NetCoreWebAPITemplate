using Application.Services.Wrappers;

namespace Application.Services.Abstractions
{
    /// <summary>
    /// Representa un manejador de solicitud
    /// </summary>
    /// <typeparam name="TRequest">Tipo de solicitud a manejar</typeparam>
    public interface IRequestHandler<TRequest> : MediatR.IRequestHandler<TRequest,Response> where TRequest : IRequest
    {
    }

    /// <summary>
    /// Representa un manejador de solicitud que devuelve un valor
    /// </summary>
    /// <typeparam name="TRequest">Tipo de solicitud a manejar</typeparam>
    /// <typeparam name="TValue">Tipo de dato devuelto</typeparam>
    public interface IRequestHandler<TRequest,TValue> : 
        MediatR.IRequestHandler<TRequest, Response<TValue>> where TRequest : IRequest<TValue>
    {
    }
}
