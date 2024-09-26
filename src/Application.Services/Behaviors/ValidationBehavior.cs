using Application.Services.Wrappers;
using FluentValidation;
using MediatR;
using System.Net;

namespace Application.Services.Behaviors
{
    /// <summary>
    /// Validador de comandos y queries
    /// </summary>
    /// <typeparam name="TRequest">Tipo de solicitud</typeparam>
    /// <typeparam name="TResponse">Tipo de respuesta</typeparam>
    /// <param name="validators">Validadores</param>
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) :
        IPipelineBehavior<TRequest, TResponse> where TResponse : Response where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationTasks = _validators.Select(v => v.ValidateAsync(context, cancellationToken));
                var validationResults = await Task.WhenAll(validationTasks);

                var errors = validationResults.Where(x => x.Errors.Count > 0)
                    .SelectMany(x => x.Errors);

                var errorMsgs = errors.Select(x => x.ErrorMessage);
                var response = Activator.CreateInstance<TResponse>();
                response.Succeeded = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Errors = errorMsgs;
                return response;
            }

            return await next();
        }
    }
}
