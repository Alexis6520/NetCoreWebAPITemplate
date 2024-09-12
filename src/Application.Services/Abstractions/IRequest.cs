using Application.Services.Wrappers;

namespace Application.Services.Abstractions
{
    /// <summary>
    /// Representa una solicitud
    /// </summary>
    public interface IRequest : MediatR.IRequest<Response>
    {
    }

    /// <summary>
    /// Representa una solicitud que espera un valor
    /// </summary>
    /// <typeparam name="T">Tipo de valor esperado</typeparam>
    public interface IRequest<T> : MediatR.IRequest<Response<T>>
    {
    }
}
