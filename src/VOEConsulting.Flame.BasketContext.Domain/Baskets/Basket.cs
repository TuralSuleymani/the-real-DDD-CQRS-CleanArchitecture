using VOEConsulting.Flame.BasketContext.Domain.Baskets.Events;
using VOEConsulting.Flame.BasketContext.Domain.Baskets.Services;
using VOEConsulting.Flame.BasketContext.Domain.Coupons;
using VOEConsulting.Flame.Common.Domain.Exceptions;

namespace VOEConsulting.Flame.BasketContext.Domain.Baskets
{
    public sealed class Basket : AggregateRoot<Basket>
    {
        public IDictionary<Seller, (IList<BasketItem> Items, decimal ShippingAmountLeft)> BasketItems { get; private set; }
        public decimal TaxPercentage { get; }
        public decimal TotalAmount { get; private set; }
        public Customer Customer { get; private set; }
        public Id<Coupon>? CouponId { get; private set; } = null;

        private Basket(decimal taxPercentage, Customer customer)
        {
            BasketItems = new Dictionary<Seller, (IList<BasketItem>, decimal)>();
            TaxPercentage = taxPercentage.EnsurePositive();
            TotalAmount = 0;
            Customer = customer;
        }

        public void AddItem(BasketItem basketItem)
        {
            if (BasketItems.TryGetValue(basketItem.Seller, out (IList<BasketItem> Items, decimal ShippingAmountLeft) value))
            {
                value.Items.Add(basketItem);
            }
            else
                BasketItems.Add(basketItem.Seller, (new List<BasketItem> { basketItem }, basketItem.Seller.ShippingLimit));

            RaiseDomainEvent(new BasketItemAddedEvent(this.Id, basketItem));
        }

        public static Basket Create(decimal taxPercentage, Customer customer)
        {
            var basket = new Basket(taxPercentage, customer);
            //basket.RaiseDomainEvent(new BasketCreatedEvent(basket.Id, customer.Id));
            return basket;
        }

        public void UpdateItemCount(BasketItem basketItem, int count)
        {
            BasketItems.EnsureKeyExists(basketItem.Seller);

            var existingBasketItem = BasketItems[basketItem.Seller].Items.FirstOrDefault(x => x.Id == basketItem.Id);
            
            existingBasketItem.EnsureNonNull();

            existingBasketItem!.UpdateCount(count);

            RaiseDomainEvent(new BasketItemCountUpdatedEvent(this.Id, basketItem, count));
        }

        public void DeleteItem(BasketItem basketItem)
        {
            BasketItems.EnsureKeyExists(basketItem.Seller);

            var items = BasketItems[basketItem.Seller].Items;
            items.EnsureNonNull();

            items.Remove(basketItem);

            RaiseDomainEvent(new BasketItemDeletedEvent(this.Id, basketItem!));
        }

        public void DeleteAll()
        {
            BasketItems.Clear();
            RaiseDomainEvent(new BasketItemsDeletedEvent(this.Id));
        }

        public void CalculateShippingAmount(Seller seller)
        {
            // Calculate the total amount
            decimal totalAmount = CalculateSellerAmount(seller);

            var (Items, ShippingAmountLeft) = BasketItems[seller];

            // Determine and update shipping amount left
            ShippingAmountLeft = totalAmount > seller.ShippingLimit
                ? 0 // No shipping cost
                : ShippingAmountLeft - totalAmount;

            RaiseDomainEvent(new ShippingAmountCalculatedEvent(this.Id, seller, ShippingAmountLeft));
        }

        public void CalculateBasketItemsAmount()
        {
            decimal totalBasketItemsAmount = 0;

            if (BasketItems.Count > 0)
            {
                foreach (var seller in BasketItems.Keys)
                {
                    totalBasketItemsAmount += CalculateSellerAmount(seller);
                }
            }

            RaiseDomainEvent(new BasketItemsAmountCalculatedEvent(this.Id, totalBasketItemsAmount));
        }

        public async Task CalculateTotalAmount(ICouponService couponService)
        {
            decimal totalAmount = CalculateTotalBasketAmount();
            totalAmount = await ApplyCouponDiscount(totalAmount, couponService);
            totalAmount = ApplyEliteMemberDiscount(totalAmount);
            TotalAmount = ApplyTax(totalAmount);

            RaiseDomainEvent(new TotalAmountCalculatedEvent(this.Id, totalAmount));
        }
        public void AssignCustomer(Customer customer)
        {
            Customer = customer;
            RaiseDomainEvent(new CustomerAssignedEvent(this.Id, customer));
        }

        public void DeactivateBasketItem(BasketItem basketItem)
        {
            basketItem.IsActive.EnsureTrue();

            BasketItems.EnsureKeyExists(basketItem.Seller);

            var items = BasketItems[basketItem.Seller].Items;
            items.EnsureNonNull();

            var existingBasketItem = items.FirstOrDefault(x => x.Id == basketItem.Id);
            existingBasketItem.EnsureNonNull();

            existingBasketItem!.Deactivate();

            RaiseDomainEvent(new BasketItemDeactivatedEvent(this.Id, basketItem));
        }

        public void ActivateBasketItem(BasketItem basketItem)
        {
            basketItem.IsActive.EnsureFalse();

            BasketItems.EnsureKeyExists(basketItem.Seller);

            var items = BasketItems[basketItem.Seller].Items;
            items.EnsureNonNull();

            var existingBasketItem = items.FirstOrDefault(x => x.Id == basketItem.Id);
            existingBasketItem.EnsureNonNull();

            existingBasketItem!.Activate();

            RaiseDomainEvent(new BasketItemActivatedEvent(this.Id, basketItem));
        }

        public async Task ApplyCoupon(Id<Coupon> couponId, ICouponService couponService)
        {
            if (CouponId == couponId)
                return; // Already applied, no action needed.

            if (!await couponService.IsActive(couponId))
            {
                throw new ValidationException("Coupon is not active!");
            }

            CouponId = couponId;
            RaiseDomainEvent(new CouponAppliedEvent(this.Id, couponId));
        }

        public void RemoveCoupon()
        {
            CouponId.EnsureNonNull();
            var couponId = CouponId;
            CouponId = null;
            RaiseDomainEvent(new CouponRemovedEvent(this.Id, couponId!));
        }
        private async Task<decimal> ApplyCouponDiscount(decimal totalAmount, ICouponService couponService)
        {
            if (CouponId is not null)
            {
                return await couponService.ApplyDiscountAsync(CouponId, totalAmount);
            }

            return totalAmount;
        }

        private decimal ApplyEliteMemberDiscount(decimal totalAmount)
        {
            return Customer.IsEliteMember ? totalAmount - (totalAmount * Customer.DiscountPercentage) : totalAmount;
        }

        private decimal CalculateTotalBasketAmount()
        {
            decimal totalAmount = 0;

            foreach (var seller in BasketItems.Keys)
            {
                decimal totalAmountBySeller = CalculateSellerAmount(seller);
                decimal costOfShipping = CalculateShippingCost(seller, totalAmountBySeller);

                totalAmount += costOfShipping + totalAmountBySeller;
            }

            return totalAmount;
        }

        private static decimal CalculateShippingCost(Seller seller, decimal totalAmountBySeller)
        {
            return totalAmountBySeller > seller.ShippingLimit ? 0 : seller.ShippingCost;
        }

        private decimal ApplyTax(decimal amount)
        {
            return amount + ((amount * TaxPercentage) / 100);
        }

        private decimal CalculateSellerAmount(Seller seller)
        {
            var (Items, ShippingAmountLeft) = BasketItems.EnsureKeyExists(seller);

            var items = Items.EnsureNonNull();

            decimal totalAmount = items.Where(x => x.IsActive).Sum(basketItem => basketItem.Quantity.TotalPrice);

            return totalAmount;
        }
    }
}
