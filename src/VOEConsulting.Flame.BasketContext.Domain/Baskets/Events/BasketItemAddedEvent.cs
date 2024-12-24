namespace VOEConsulting.Flame.BasketContext.Domain.Baskets.Events
{
    public sealed class BasketItemAddedEvent : BaseBasketDomainEvent
    {
        public BasketItemAddedEvent(Id<Basket> basketId, BasketItem basketItem)
               : base(basketId)
        {
            BasketItem = basketItem;
        }
        public BasketItem BasketItem { get; }
    }
}
