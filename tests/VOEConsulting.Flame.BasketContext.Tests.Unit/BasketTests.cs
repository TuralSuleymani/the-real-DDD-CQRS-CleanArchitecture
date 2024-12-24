using NSubstitute;
using VOEConsulting.Flame.Common.Domain.Exceptions;
using VOEConsulting.Flame.BasketContext.Domain.Baskets.Services;
using VOEConsulting.Flame.BasketContext.Domain.Coupons;
using VOEConsulting.Flame.BasketContext.Tests.Data;
using VOEConsulting.Flame.BasketContext.Tests.Unit.Extensions;
using System.Diagnostics;

namespace VOEConsulting.Flame.BasketContext.Tests.Unit
{
    public class BasketTests
    {
        [Theory]
        [InlineData(.18)]
        [InlineData(.24)]
        [InlineData(.45)]
        public void Create_WhenValidArgumentProvided_ShouldCreateBasket(decimal taxPercentage)
        {
            //Arrange
            var customer = TestFactories.CustomerFactory.Create();

            //Act
            Basket basket = Basket.Create(taxPercentage, customer);

            //Assert
            basket.Should().NotBeNull();
        }

        [Fact]
        public void AddItem_WhenBasketItemIsAdded_ShouldRaiseBasketItemAddedEvent()
        {
            // Arrange
            Basket basket = TestFactories.BasketFactory.Create();
            BasketItem basketItem = TestFactories.BasketItemFactory.Create();

            var expectedEvent = new BasketItemAddedEvent(basket.Id, basketItem);

            // Act
            basket.AddItem(basketItem);

            // Assert
            var actualEvent = basket.DomainEvents.Single();
            actualEvent.Should().BeEquivalentEventTo(expectedEvent);
        }

        [Fact]
        public void UpdateItemCount_WhenBasketItemDoesNotExist_ShouldFail()
        {
            // Arrange
            Basket basket = TestFactories.BasketFactory.Create();
            var basketItem = TestFactories.BasketItemFactory.Create();

            // Act
            var action = () => basket.UpdateItemCount(basketItem, 5);

            // Assert
            action.Should().ThrowExactly<ValidationException>();
        }

        [Fact]
        public void UpdateItemCount_WhenBasketItemExists_ShouldRaiseBasketItemCountUpdatedEvent()
        {
            // Arrange
            Basket basket = TestFactories.BasketFactory.Create();
            var basketItem = TestFactories.BasketItemFactory.Create();

            var expectedEvent = new BasketItemCountUpdatedEvent(basket.Id, basketItem, 5);

            basket.AddItem(basketItem);

            // Act
            basket.UpdateItemCount(basketItem, 5);

            // Assert
            basket.DomainEvents.Should().HaveCount(2);
            basket.DomainEvents.Last().Should().BeEquivalentEventTo(expectedEvent);
        }

        [Fact]
        public void DeleteItem_WhenBasketItemDoesNotExist_ShouldFail()
        {
            // Arrange
            Basket basket = TestFactories.BasketFactory.Create();
            var basketItem = TestFactories.BasketItemFactory.Create();

            // Act
            var action = () => basket.DeleteItem(basketItem);

            // Assert
            action.Should().ThrowExactly<ValidationException>();
        }

        [Fact]
        public void DeleteItem_WhenBasketItemExists_ShouldRaiseBasketItemDeletedEvent()
        {
            // Arrange
            Basket basket = TestFactories.BasketFactory.Create();
            var basketItem = TestFactories.BasketItemFactory.Create();

            var expectedEvent = new BasketItemDeletedEvent(basket.Id, basketItem);

            basket.AddItem(basketItem);

            // Act
            basket.DeleteItem(basketItem);

            // Assert
            basket.DomainEvents.Should().HaveCount(2);
            basket.DomainEvents.Last().Should().BeEquivalentEventTo(expectedEvent);
        }

        [Theory]
        [InlineData(300, 45, new object[] { 100.0, 100.0 })]
        [InlineData(500, 120, new object[] { 100.0, 100.0 })]
        [InlineData(120, 34, new object[] { 100.0, 100.0 })]
        [InlineData(185, 30, new object[] { 100.0, 100.0 })]
        public void CalculateShippingAmount_WhenBasketItemIsUnderLimit_ShouldRaiseShippingAmountCalculatedEvent(
          decimal shippingLimit, decimal shippingCost, object[] basketItemPrices)
        {
            // Arrange
            var basket = TestFactories.BasketFactory.Create();
            var seller = TestFactories.SellerFactory.Create(shippingLimit: shippingLimit, shippingCost: shippingCost);

            List<decimal> decimalList = basketItemPrices.OfType<double>()
                .Select(x => (decimal)x)
                .ToList();


            // Add basket items to the basket
            decimalList.ForEach(price =>
            {
                var quantity = TestFactories.QuantityFactory.Create(pricePerUnit: price);
                var basketItem = TestFactories.BasketItemFactory.Create(quantity: quantity, seller: seller);
                basket.AddItem(basketItem);
            });

            // Calculate total item price
            var totalItemPrice = decimalList.Sum();

            // Determine shipping amount
            var shippingAmount = totalItemPrice > shippingLimit ? 0 : shippingLimit - totalItemPrice;

            // Create the expected event
            var expectedEvent = new ShippingAmountCalculatedEvent(basket.Id, seller, shippingAmount);

            // Act
            basket.CalculateShippingAmount(seller);

            // Assert
            basket.DomainEvents.Should().HaveCount(3);
            basket.DomainEvents.Last().Should().BeEquivalentEventTo(expectedEvent);
        }


        [Fact]
        public void CalculateShippingAmount_WhenSellerArgumentIsNull_ShouldFail()
        {
            // Arrange
            Basket basket = TestFactories.BasketFactory.Create();

            // Act
            var action = () => basket.CalculateShippingAmount(null!);

            // Assert
            action.Should().ThrowExactly<ValidationException>();
        }

        [Fact]
        public void CalculateShippingAmount_WhenSellerIsValidBasketIsEmpty_ShouldFail()
        {
            // Arrange
            Basket basket = TestFactories.BasketFactory.Create();
            Seller seller = TestFactories.SellerFactory.Create();

            // Act
            var action = () => basket.CalculateShippingAmount(seller);

            // Assert
            action.Should().ThrowExactly<ValidationException>();
        }

        [Fact]
        public void CalculateShippingAmount_WhenSellersBasketItemsIsNull_ShouldFail()
        {
            // Arrange
            Basket basket = TestFactories.BasketFactory.Create();
            Seller seller = TestFactories.SellerFactory.Create();
            basket.BasketItems.Add(seller, (null, 0m));

            // Act
            var action = () => basket.CalculateShippingAmount(seller);

            // Assert
            action.Should().ThrowExactly<ValidationException>();
        }

        [Fact]
        public void CalculateBasketItemsAmount_WhenBasketItemsIsEmpty_ShouldRaiseBasketItemsAmountCalculatedEventWithZeroAmount()
        {
            // Arrange
            Basket basket = TestFactories.BasketFactory.Create();
            var expectedEvent = new BasketItemsAmountCalculatedEvent(basket.Id, 0);

            // Act
            basket.CalculateBasketItemsAmount();

            // Assert
            basket.DomainEvents.Should().HaveCount(1);
            basket.DomainEvents.Last().Should().BeEquivalentEventTo(expectedEvent);
        }

        [Fact]
        public void CalculateBasketItemsAmount_WhenBasketItemsIsEmpty_ShouldRaiseBasketItemsAmountCalculatedEventWithAmount()
        {
            // Arrange
            Basket basket = TestFactories.BasketFactory.Create();

            BasketItem basketItem = TestFactories.BasketItemFactory.Create();
            basket.AddItem(basketItem);

            var expectedEvent = new BasketItemsAmountCalculatedEvent(basket.Id, basketItem.Quantity.TotalPrice);

            // Act
            basket.CalculateBasketItemsAmount();

            // Assert
            basket.DomainEvents.Should().HaveCount(2);
            basket.DomainEvents.Last().Should().BeEquivalentEventTo(expectedEvent);
        }


        [Theory]
        [InlineData(200, 50, new object[] { 100.0, 400.0 })]
        [InlineData(500, 60, new object[] { 400, 150 })]
        public async Task CalculateTotalAmount_WhenBasketIsNotEmpty_ShouldCalculateTotalAmount(
            int shippingAmount, decimal shippingCost, object[] basketItemPrices)
        {
            // Arrange
            Basket basket = TestFactories.BasketFactory.Create();
            Seller seller = TestFactories.SellerFactory.Create(shippingLimit: shippingAmount, shippingCost: shippingCost);

            List<decimal> decimalList = basketItemPrices.OfType<double>()
                .Select(x => (decimal)x)
                .ToList();

            // Add basket items to the basket
            decimalList.ForEach(price =>
            {
                var quantity = TestFactories.QuantityFactory.Create(pricePerUnit: price);
                var basketItem = TestFactories.BasketItemFactory.Create(quantity: quantity, seller: seller);
                basket.AddItem(basketItem);
            });

            var total = decimalList.Sum();
            var expectedTotalAmount = total + (total * 18 / 100);

            // Act
            await basket.CalculateTotalAmount(null!);

            // Assert
            basket.TotalAmount.Should().Be(expectedTotalAmount);
        }

        [Theory]
        [InlineData(200, 50, new object[] { 100.0, 400.0 })]
        [InlineData(500, 60, new object[] { 400, 150 })]
        public async Task CalculateTotalAmount_WhenBasketCustomerIsEliteMember_ShouldCalculateTotalAmountWithEliteDiscount(
            int shippingAmount, decimal shippingCost, object[] basketItemPrices)
        {
            // Arrange
            Customer eliteCustomer = CustomerData.EliteCustomer;
            Basket basket = TestFactories.BasketFactory.Create(customer: eliteCustomer);
            Seller seller = TestFactories.SellerFactory.Create(shippingLimit: shippingAmount, shippingCost: shippingCost);

            List<decimal> decimalList = basketItemPrices.OfType<double>()
                .Select(x => (decimal)x)
                .ToList();

            // Add basket items to the basket
            decimalList.ForEach(price =>
            {
                var quantity = TestFactories.QuantityFactory.Create(pricePerUnit: price);
                var basketItem = TestFactories.BasketItemFactory.Create(quantity: quantity, seller: seller);
                basket.AddItem(basketItem);
            });

            var total = decimalList.Sum();
            total = total - (total * eliteCustomer.DiscountPercentage);
            var expectedTotalAmount = total + (total * 18 / 100);

            // Act
            await basket.CalculateTotalAmount(null!);

            // Assert
            basket.TotalAmount.Should().Be(expectedTotalAmount);
        }

        [Fact]
        public void AssignCustomer_WhenCustomerIsLogged_ShouldRaiseCustomerAssignedEvent()
        {
            // Arrange
            Basket basket = TestFactories.BasketFactory.Create();
            Customer customer = TestFactories.CustomerFactory.Create();

            // Act
            basket.AssignCustomer(customer);

            // Assert
            basket.Customer.Should().Be(customer);
        }

        [Fact]
        public void DeactivateBasketItem_WhenBasketItemExists_ShouldRaiseBasketItemDeactivatedEvent()
        {
            // Arrange
            Basket basket = TestFactories.BasketFactory.Create();
            BasketItem basketItem = TestFactories.BasketItemFactory.Create();

            var expectedEvent = new BasketItemDeactivatedEvent(basket.Id, basketItem);

            basket.AddItem(basketItem);

            // Act
            basket.DeactivateBasketItem(basketItem);

            // Assert
            basket.DomainEvents.Last().Should().BeEquivalentEventTo(expectedEvent);
        }

        [Fact]
        public void DeactivateBasketItem_WhenBasketItemDoesNotExist_ShouldFail()
        {
            // Arrange
            Basket basket = TestFactories.BasketFactory.Create();
            BasketItem basketItem = TestFactories.BasketItemFactory.Create();

            // Act
            var action = () => basket.DeactivateBasketItem(basketItem);

            // Assert
            action.Should().ThrowExactly<ValidationException>();
        }

        [Fact]
        public void DeactivateBasketItem_WhenBasketItemIsAlreadyDeactivated_ShouldFail()
        {
            // Arrange
            Basket basket = TestFactories.BasketFactory.Create();
            BasketItem basketItem = TestFactories.BasketItemFactory.Create();

            basket.AddItem(basketItem);
            basket.DeactivateBasketItem(basketItem);

            // Act
            var action = () => basket.DeactivateBasketItem(basketItem);

            // Assert
            action.Should().ThrowExactly<ValidationException>();
        }

        [Fact]
        public void ActivateBasketItem_WhenBasketItemExists_ShouldRaiseBasketItemActivatedEvent()
        {
            // Arrange
            Basket basket = TestFactories.BasketFactory.Create();
            BasketItem basketItem = TestFactories.BasketItemFactory.Create();

            var expectedEvent = new BasketItemActivatedEvent(basket.Id, basketItem);

            basket.AddItem(basketItem);
            basket.DeactivateBasketItem(basketItem);

            // Act
            basket.ActivateBasketItem(basketItem);

            // Assert
            basket.DomainEvents.Last().Should().BeEquivalentEventTo(expectedEvent);
        }

        [Fact]
        public void ActivateBasketItem_WhenBasketItemDoesNotExist_ShouldFail()
        {
            // Arrange
            Basket basket = TestFactories.BasketFactory.Create();
            BasketItem basketItem = TestFactories.BasketItemFactory.Create();

            // Act
            var action = () => basket.ActivateBasketItem(basketItem);

            // Assert
            action.Should().ThrowExactly<ValidationException>();
        }

        [Fact]
        public void ActivateBasketItem_WhenBasketItemIsAlreadyActivated_ShouldFail()
        {
            // Arrange
            Basket basket = TestFactories.BasketFactory.Create();
            BasketItem basketItem = TestFactories.BasketItemFactory.Create();

            basket.AddItem(basketItem);

            // Act
            var action = () => basket.ActivateBasketItem(basketItem);

            // Assert
            action.Should().ThrowExactly<ValidationException>();
        }

        [Theory]
        [InlineData(200, 50, new object[] { 100.0, 400.0 }, 90)]
        [InlineData(500, 60, new object[] { 400, 150 }, 50)]
        public async Task CalculateTotalAmount_WhenHasActiveCouponWithFixValue_ShouldCalculateTotalAmountWithCoupon(
            int shippingAmount, decimal shippingCost, object[] basketItemPrices, decimal fixCouponAmount)
        {
            // Arrange
            Basket basket = TestFactories.BasketFactory.Create();
            var coupon = TestFactories.CouponFactory.Create(amount: Amount.Fix(fixCouponAmount));

            var couponService = Substitute.For<ICouponService>();

            couponService.IsActive(coupon.Id).Returns(Task.FromResult(true));

            await basket.ApplyCoupon(coupon.Id, couponService);

            Seller seller = TestFactories.SellerFactory.Create(shippingLimit: shippingAmount, shippingCost: shippingCost);

            List<decimal> decimalList = basketItemPrices.OfType<double>()
                .Select(x => (decimal)x)
                .ToList();

            // Add basket items to the basket
            decimalList.ForEach(price =>
            {
                var quantity = TestFactories.QuantityFactory.Create(pricePerUnit: price);
                var basketItem = TestFactories.BasketItemFactory.Create(quantity: quantity, seller: seller);
                basket.AddItem(basketItem);
            });

            var total = decimalList.Sum();



            couponService.ApplyDiscountAsync(coupon.Id, total)
                .Returns(Task.FromResult(total - coupon.Amount.Value));

            total = await couponService.ApplyDiscountAsync(coupon.Id, total);

            var expectedTotalAmount = total + (total * 18 / 100);


            // Act
            await basket.CalculateTotalAmount(couponService);

            // Assert
            basket.TotalAmount.Should().Be(expectedTotalAmount);
        }


        [Theory]
        [InlineData(200, 50, new object[] { 100.0, 400.0 }, .9)]
        [InlineData(500, 60, new object[] { 400, 150 }, .13)]
        public async Task CalculateTotalAmount_WhenHasActiveCouponWithPercentage_ShouldCalculateTotalAmountWithCoupon(
           int shippingAmount, decimal shippingCost, object[] basketItemPrices, decimal percentageCouponAmount)
        {
            // Arrange
            Basket basket = TestFactories.BasketFactory.Create();
            var coupon = TestFactories.CouponFactory.Create(amount: Amount.Percentage(percentageCouponAmount));

            var couponService = NSubstitute.Substitute.For<ICouponService>();
            couponService.IsActive(coupon.Id).Returns(Task.FromResult(true));

            await basket.ApplyCoupon(coupon.Id, couponService);

            Seller seller = TestFactories.SellerFactory.Create(shippingLimit: shippingAmount, shippingCost: shippingCost);

            List<decimal> decimalList = basketItemPrices.OfType<double>()
                .Select(x => (decimal)x)
                .ToList();

            // Add basket items to the basket
            decimalList.ForEach(price =>
            {
                var quantity = TestFactories.QuantityFactory.Create(pricePerUnit: price);
                var basketItem = TestFactories.BasketItemFactory.Create(quantity: quantity, seller: seller);
                basket.AddItem(basketItem);
            });

            var total = decimalList.Sum();

            couponService.ApplyDiscountAsync(coupon.Id, total)
                .Returns(Task.FromResult(total - (total * coupon.Amount.Value)));

            total = await couponService.ApplyDiscountAsync(coupon.Id, total);

            var expectedTotalAmount = total + (total * 18 / 100);

            // Act
            await basket.CalculateTotalAmount(couponService);

            // Assert
            basket.TotalAmount.Should().Be(expectedTotalAmount);
        }

        [Fact]
        public async Task ApplyCoupon_WhenCouponIsNotActive_ShouldFail()
        {
            // Arrange
            Basket basket = TestFactories.BasketFactory.Create();
            var coupon = TestFactories.CouponFactory.Create();
            coupon.Deactivate();

            var couponService = NSubstitute.Substitute.For<ICouponService>();

            couponService.IsActive(coupon.Id).Returns(Task.FromResult(coupon.IsActive));

            // Act
            var action = async () => await basket.ApplyCoupon(coupon.Id, couponService);

            // Assert
            await action.Should().ThrowExactlyAsync<ValidationException>();
        }

        [Fact]
        public void RemoveCoupon_WhenCouponIdIsNull_ShouldFail()
        {
            // Arrange
            Basket basket = TestFactories.BasketFactory.Create();

            // Act
            var action = () => basket.RemoveCoupon();

            // Assert
            action.Should().ThrowExactly<ValidationException>();
        }

        [Fact]
        public async Task RemoveCoupon_WhenCouponIdIsNotNull_ShouldRemoveCoupon()
        {
            // Arrange
            Basket basket = TestFactories.BasketFactory.Create();
            var coupon = TestFactories.CouponFactory.Create();

            var couponService = NSubstitute.Substitute.For<ICouponService>();

            couponService.IsActive(coupon.Id).Returns(Task.FromResult(coupon.IsActive));

            await basket.ApplyCoupon(coupon.Id, couponService);

            // Act
            basket.RemoveCoupon();

            // Assert
            basket.CouponId.Should().BeNull();
        }

    }
}
