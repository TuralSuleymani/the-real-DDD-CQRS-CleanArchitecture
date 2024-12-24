using MediatR;

namespace VOEConsulting.Flame.BasketContext.Application.Abstractions
{
    public interface ICommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, Result<TResponse,IDomainError>>
        where TRequest : ICommand<TResponse>
        where TResponse : notnull
    { }

    public interface ICommandHandler<TRequest> : IRequestHandler<TRequest, Result<Unit>>
        where TRequest : ICommand
    { }
}
