namespace VOEConsulting.Flame.BasketContext.Application.Coupons
{
    public record ApplyCouponCommand(Guid BasketId, Guid CouponId) : IQuery;

}
