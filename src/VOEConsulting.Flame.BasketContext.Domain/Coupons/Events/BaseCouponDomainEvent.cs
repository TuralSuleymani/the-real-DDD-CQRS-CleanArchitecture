using VOEConsulting.Flame.BasketContext.Domain.Common;

namespace VOEConsulting.Flame.BasketContext.Domain.Coupons.Events
{
    [AggregateType(BasketEventConstants.CouponsAggregateTypeName)]
    public abstract class BaseCouponDomainEvent(Id<Coupon> aggregateId, DateTimeOffset? occurredOnUtc = null)
        : DomainEvent(aggregateId, occurredOnUtc ?? DateTimeOffset.UtcNow) { }
}
