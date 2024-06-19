using FluentValidation;
using MediatR;
using Services.Wrappers;
using System.Net;

namespace Logic.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TResponse : Result where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null);

                if (failures.Any())
                {
                    var errors = failures.Select(x => x.ErrorMessage);
                    return (TResponse)Activator.CreateInstance(typeof(TResponse), false, HttpStatusCode.BadRequest, errors)!;
                }

            }

            return await next();
        }
    }
}
