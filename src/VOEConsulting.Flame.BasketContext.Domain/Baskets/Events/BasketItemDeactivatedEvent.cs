namespace VOEConsulting.Flame.BasketContext.Domain.Baskets.Events
{
    public sealed class BasketItemDeactivatedEvent : BaseBasketDomainEvent
    {
        public BasketItemDeactivatedEvent(Id<Basket> basketId, BasketItem basketItem)
               : base(basketId)
        {
            BasketItem = basketItem;
        }
        public BasketItem BasketItem { get; }
    }
}