using FluentValidation;
using MediatR;
using Validation = VOEConsulting.Flame.Common.Domain.Exceptions;

namespace VOEConsulting.Flame.BasketContext.Application.Behaviours
{
    public class ValidationPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : notnull, IRequest<TResponse>
         where TResponse : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPipelineBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any()) return await next();

            var validationContext = new ValidationContext<TRequest>(request);
            var validationResponse = await Task.WhenAll(_validators.Select(x => x.ValidateAsync(validationContext, cancellationToken)));

            var validationErrors = validationResponse
                .SelectMany(x => x.Errors)
                .Where(e => e != null)
                .Select(x => x.ErrorMessage)
                .ToList();

            if (validationErrors.Any()) throw new Validation.ValidationException(validationErrors);

            return await next();
        }
    }

}
