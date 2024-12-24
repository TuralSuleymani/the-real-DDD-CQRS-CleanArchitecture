using NSubstitute;
using VOEConsulting.Flame.BasketContext.Domain.Coupons;
using VOEConsulting.Flame.BasketContext.Domain.Coupons.Events;
using VOEConsulting.Flame.BasketContext.Tests.Unit.Extensions;
using VOEConsulting.Flame.Common.Domain;
using VOEConsulting.Flame.Common.Domain.Exceptions;
using VOEConsulting.Flame.Common.Domain.Services;

namespace VOEConsulting.Flame.BasketContext.Tests.Unit
{
    public class CouponTests
    {
        [Theory]
        [InlineData("TESTCOUPON", 20, "2024-10-10", "2024-11-11")]
        public void Create_WhenValidCouponDataProvided_ShouldRaiseCouponCreatedEvent(
            string code, decimal value, string startDate, string endDate)
        {
            // Arrange&Act
            var coupon = Coupon.Create(code, Amount.Percentage(value), DateRange.FromString(startDate, endDate));
            var expectedEvent = new CouponCreatedEvent(coupon.Id);

            // Assert
            var actualEvent = coupon.DomainEvents.Single();
            actualEvent.Should().BeEquivalentEventTo(expectedEvent);
        }

        [Theory]
        [InlineData("TESTCOUPON", 20, "2024-10-10", "2024-11-11")]
        [InlineData("TESTCPN", 56, "2022-10-10", "2023-06-11")]
        public void Create_WhenValidCouponDataProvided_ShouldReturnCouponWithCorrectProperties(
            string code, decimal value, string startDate, string endDate)
        {
            //Arrange

            Amount amount = Amount.Percentage(value);
            DateRange dateRange = DateRange.FromString(startDate, endDate);

            // Act
            var coupon = Coupon.Create(code, amount, dateRange);

            // Assert
            coupon.Code.Should().Be(code);
            coupon.Amount.Should().Be(amount);
            coupon.ValidityPeriod.Should().Be(dateRange);
            coupon.IsActive.Should().BeTrue();
        }

        [Theory]
        [InlineData("TST", 20, "2024-10-10", "2024-11-11")]
        [InlineData("TEST", 56, "2022-10-10", "2023-06-11")]
        public void Create_WhenInValidCouponCodeProvided_ShouldFail(
            string code, decimal value, string startDate, string endDate)
        {
            //Arrange
            Amount amount = Amount.Percentage(value);
            DateRange dateRange = DateRange.FromString(startDate, endDate);

            // Act
            var action = () => Coupon.Create(code, amount, dateRange);

            // Assert
            action.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Deactivate_WhenCouponIsActive_ShouldRaiseCouponDeactivatedEvent()
        {
            // Arrange
            var coupon = TestFactories.CouponFactory.Create();

            var expectedEvent = new CouponDeactivatedEvent(coupon.Id);

            // Act
            coupon.Deactivate();

            // Assert
            coupon.DomainEvents.Last().Should().BeEquivalentEventTo(expectedEvent);
        }

        [Fact]
        public void Deactivate_WhenCouponIsAlreadyDeactive_ShouldFail()
        {
            // Arrange
            var coupon = TestFactories.CouponFactory.Create();
            coupon.Deactivate();

            // Act
            var action = () => coupon.Deactivate();

            // Assert
            action.Should().ThrowExactly<ValidationException>();
        }

        [Theory]
        [InlineData("2024-02-01", "2024-11-11", "2024-08-11")]
        public void Activate_WhenCouponIsNotActiveAndDateIsInRange_ShouldRaiseCouponActivatedEvent(
            string startDate, string endDate, string couponActivateDate)
        {
            // Arrange
            var validDateRange = DateRange.FromString(startDate, endDate);
            var coupon = TestFactories.CouponFactory.Create(dateRange: validDateRange);
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            dateTimeProvider.UtcNow().Returns(DateTime.Parse(couponActivateDate));

            var expectedEvent = new CouponActivatedEvent(coupon.Id);

            coupon.Deactivate();

            // Act
            coupon.Activate(dateTimeProvider);

            // Assert
            coupon.DomainEvents.Last().Should().BeEquivalentEventTo(expectedEvent);
        }

    }
}
