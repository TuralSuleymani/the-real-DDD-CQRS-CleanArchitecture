namespace VOEConsulting.Flame.BasketContext.Domain.Baskets.Events
{
    public sealed class BasketItemDeletedEvent : BaseBasketDomainEvent
    {
        public BasketItemDeletedEvent(Id<Basket> basketId, BasketItem basketItem)
               : base(basketId)
        {
            BasketItem = basketItem;
        }
        public BasketItem BasketItem { get; }
    }
}
