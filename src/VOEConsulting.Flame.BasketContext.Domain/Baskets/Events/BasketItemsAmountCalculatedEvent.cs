namespace VOEConsulting.Flame.BasketContext.Domain.Baskets.Events
{
    public sealed class BasketItemsAmountCalculatedEvent : BaseBasketDomainEvent
    {
        public BasketItemsAmountCalculatedEvent(Id<Basket> basketId, decimal amount)
               : base(basketId)
        {
            Amount = amount;
        }
        public decimal Amount { get; }
    }
}