using MediatR;

namespace VOEConsulting.Flame.Common.Domain.Events
{
    public interface IDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    { }
}
