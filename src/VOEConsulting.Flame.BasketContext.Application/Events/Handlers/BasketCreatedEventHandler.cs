using VOEConsulting.Flame.BasketContext.Application.Abstractions.Messaging;
using VOEConsulting.Flame.BasketContext.Application.Events.Integration;
using VOEConsulting.Flame.BasketContext.Domain.Baskets.Events;
using VOEConsulting.Flame.Common.Domain.Events;

namespace VOEConsulting.Flame.BasketContext.Application.Events.Handlers
{
    public class BasketCreatedDomainEventHandler : IDomainEventHandler<BasketCreatedEvent>
    {
        private readonly IIntegrationEventPublisher _integrationEventPublisher;

        public BasketCreatedDomainEventHandler(IIntegrationEventPublisher integrationEventPublisher)
        {
            _integrationEventPublisher = integrationEventPublisher;
        }

        public async Task Handle(BasketCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            var integrationEvent = new BasketCreatedIntegrationEvent(domainEvent.AggregateId, domainEvent.CustomerId);
            await _integrationEventPublisher.PublishAsync(integrationEvent);
        }
    }
}
