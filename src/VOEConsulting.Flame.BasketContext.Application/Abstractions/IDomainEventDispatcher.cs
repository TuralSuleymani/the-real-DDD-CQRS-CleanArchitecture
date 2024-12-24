using VOEConsulting.Flame.Common.Domain.Events;

namespace VOEConsulting.Flame.BasketContext.Application.Abstractions
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
    }
}
