using VOEConsulting.Flame.BasketContext.Domain.Coupons.Events;
using VOEConsulting.Flame.Common.Domain.Services;

namespace VOEConsulting.Flame.BasketContext.Domain.Coupons
{
    public sealed class Coupon : AggregateRoot<Coupon>
    {
        private Coupon(string code, Amount amount, DateRange validityPeriod)
        {
            Code = code.EnsureLengthInRange(6, 10);
            Amount = amount.EnsureNonNull();
            ValidityPeriod = validityPeriod.EnsureNonNull();
            IsActive = true;
        }

        public string Code { get; }
        public bool IsActive { get; private set; }
        public Amount Amount { get; }
        public DateRange ValidityPeriod { get; }

        public static Coupon Create(string code, Amount amount, DateRange dateRange)
        {
            var coupon = new Coupon(code, amount, dateRange);
            coupon.RaiseDomainEvent(new CouponCreatedEvent(coupon.Id));

            return coupon;
        }

        public void Activate(IDateTimeProvider dateTimeProvider)
        {
            IsActive.EnsureFalse();
            ValidityPeriod.InRange(dateTimeProvider.UtcNow());
            IsActive = true;

            RaiseDomainEvent(new CouponActivatedEvent(this.Id));
        }

        public void Deactivate()
        {
            IsActive.EnsureTrue();
            IsActive = false;

            RaiseDomainEvent(new CouponDeactivatedEvent(this.Id));
        }
    }
}
