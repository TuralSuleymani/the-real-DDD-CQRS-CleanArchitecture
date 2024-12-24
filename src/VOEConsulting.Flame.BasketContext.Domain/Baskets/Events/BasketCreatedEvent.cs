namespace VOEConsulting.Flame.BasketContext.Domain.Baskets.Events
{
    public sealed class BasketCreatedEvent : BaseBasketDomainEvent
    {
        public BasketCreatedEvent(Id<Basket> basketId, Guid customerId)
            : base(basketId)
        {
            CustomerId = customerId;
        }

        public Guid CustomerId { get; }
    }
}