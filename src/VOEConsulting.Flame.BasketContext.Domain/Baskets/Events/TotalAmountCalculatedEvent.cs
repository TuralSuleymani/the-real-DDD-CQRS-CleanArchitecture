namespace VOEConsulting.Flame.BasketContext.Domain.Baskets.Events
{
    public sealed class TotalAmountCalculatedEvent : BaseBasketDomainEvent
    {
        public TotalAmountCalculatedEvent(Id<Basket> basketId, decimal totalAmount)
            : base(basketId)
        {
            TotalAmount = totalAmount;
        }

        public decimal TotalAmount { get; }
    }
}