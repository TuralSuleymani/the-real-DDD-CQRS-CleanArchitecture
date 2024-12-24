namespace VOEConsulting.Flame.BasketContext.Domain.Baskets.Events
{
    public sealed class BasketItemsDeletedEvent : BaseBasketDomainEvent
    {
        public BasketItemsDeletedEvent(Id<Basket> basketId)
            : base(basketId) { }

    }
}
