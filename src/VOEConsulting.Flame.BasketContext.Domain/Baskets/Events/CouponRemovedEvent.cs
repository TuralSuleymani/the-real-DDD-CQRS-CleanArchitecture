using VOEConsulting.Flame.BasketContext.Domain.Coupons;

namespace VOEConsulting.Flame.BasketContext.Domain.Baskets.Events
{
    public sealed class CouponRemovedEvent : BaseBasketDomainEvent
    {
        public CouponRemovedEvent(Id<Basket> basketId, Id<Coupon> couponId)
            : base(basketId)
        {
            CouponId = couponId;
        }

        public Id<Coupon> CouponId { get; }
    }
}