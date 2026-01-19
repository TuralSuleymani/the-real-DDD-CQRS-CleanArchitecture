using FluentValidation;
using MediatR;
using System.Diagnostics;
using VOEConsulting.Flame.Common.Core.Exceptions;

public sealed class ExceptionHandlingPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (ValidationException ex)
        {
            Activity.Current?.SetStatus(ActivityStatusCode.Error, "validation_failed");
            Activity.Current?.AddException(ex);
            Activity.Current?.SetTag("error.type", "validation");
            Activity.Current?.SetTag("validation.error_count", ex.Errors?.Count() ?? 0);

            var domainError = DomainError.Validation(ex.Message, ex.Errors?.Select(x => x.ErrorMessage).ToList());
            return CastOrThrow(domainError, ex);
        }
        catch (FlameApplicationException ex)
        {
            Activity.Current?.SetStatus(ActivityStatusCode.Error, "bad_request");
            Activity.Current?.AddException(ex);
            Activity.Current?.SetTag("error.type", "bad_request");

            var domainError = DomainError.BadRequest(ex.Message);
            return CastOrThrow(domainError, ex);
        }
        catch (Exception ex)
        {
            Activity.Current?.SetStatus(ActivityStatusCode.Error, "unexpected");
            Activity.Current?.AddException(ex);
            Activity.Current?.SetTag("error.type", "unexpected");

            var domainError = DomainError.UnExpected("An unexpected error occurred.");
            return CastOrThrow(domainError, ex);
        }
    }

    private static TResponse CastOrThrow(IDomainError domainError, Exception ex)
    {
        var failureResult = Result.Failure<Guid, IDomainError>(domainError);

        if (failureResult is TResponse response)
        {
            return response;
        }

        throw new InvalidCastException(
            $"Failed to cast failure result to {typeof(TResponse).Name} for request {typeof(TRequest).Name}.",
            ex);
    }
}
