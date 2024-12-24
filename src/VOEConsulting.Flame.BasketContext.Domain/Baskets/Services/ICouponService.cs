using VOEConsulting.Flame.BasketContext.Domain.Coupons;

namespace VOEConsulting.Flame.BasketContext.Domain.Baskets.Services
{
    public interface ICouponService
    {
        Task<decimal> ApplyDiscountAsync(Id<Coupon> couponId, decimal totalAmount);
        Task<bool> IsActive(Id<Coupon> couponId);
    }
}