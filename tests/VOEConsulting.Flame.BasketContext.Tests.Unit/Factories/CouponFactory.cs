using VOEConsulting.Flame.BasketContext.Domain.Coupons;
using VOEConsulting.Flame.BasketContext.Tests.Data;
using VOEConsulting.Flame.Common.Domain;
using static VOEConsulting.Flame.BasketContext.Tests.Data.CouponData;
namespace VOEConsulting.Flame.BasketContext.Tests.Unit.Factories
{
    public static class CouponFactory
    {
        public static Coupon Create(string? code = null, Amount? amount = null, DateRange? dateRange = null)
        {
            return Coupon.Create(code ?? Code, amount ?? PercentageAmount, dateRange ?? CouponData.Range);
        }
    }
}
