using Ardalis.SmartEnum;

namespace VOEConsulting.Flame.BasketContext.Domain.Coupons
{
    public sealed class Amount : ValueObject
    {
        private Amount(decimal value, CouponType couponType)
        {
            Value = value.EnsureGreaterThan(0);
            CouponType = couponType.EnsureNonNull();
        }

        public static Amount Fix(decimal value) => new(value, CouponType.Fix);
        public static Amount Percentage(decimal value) => new(value, CouponType.Percentage);

        public decimal Value { get; }
        public CouponType CouponType { get; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
            yield return CouponType;
        }
    }

    public sealed class CouponType : SmartEnum<CouponType>
    {
        public static readonly CouponType Fix = new CouponType(nameof(Fix), 1);
        public static readonly CouponType Percentage = new CouponType(nameof(Percentage), 2);

        private CouponType(string name, int value) : base(name, value) { }
    }
}
