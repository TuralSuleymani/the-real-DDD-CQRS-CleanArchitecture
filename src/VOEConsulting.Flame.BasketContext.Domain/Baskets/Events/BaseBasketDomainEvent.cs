using VOEConsulting.Flame.BasketContext.Domain.Common;

namespace VOEConsulting.Flame.BasketContext.Domain.Baskets.Events
{
    [AggregateType(BasketEventConstants.BasketsAggregateTypeName)]
    public abstract class BaseBasketDomainEvent(Guid aggregateId, DateTimeOffset? occurredOnUtc = null)
        : DomainEvent(aggregateId, occurredOnUtc ?? DateTimeOffset.UtcNow) { }
}
