namespace VOEConsulting.Flame.BasketContext.Domain.Baskets.Events
{
    public sealed class ShippingAmountCalculatedEvent : BaseBasketDomainEvent
    {
        public ShippingAmountCalculatedEvent(Id<Basket> basketId, Seller seller, decimal shippingAmountLeft)
            : base(basketId)
        {
            Seller = seller;
            ShippingAmountLeft = shippingAmountLeft;
        }

        public Seller Seller { get; }
        public decimal ShippingAmountLeft { get; }
    }
}