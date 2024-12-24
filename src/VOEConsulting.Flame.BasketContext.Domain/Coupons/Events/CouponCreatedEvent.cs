namespace VOEConsulting.Flame.BasketContext.Domain.Coupons.Events
{
    public sealed class CouponCreatedEvent : BaseCouponDomainEvent
    {
        public CouponCreatedEvent(Id<Coupon> couponId) : base(couponId) { }
    }
}