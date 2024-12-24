namespace VOEConsulting.Flame.BasketContext.Domain.Baskets.Events
{
    public sealed class BasketItemCountUpdatedEvent : BaseBasketDomainEvent
    {
        public BasketItemCountUpdatedEvent(Id<Basket> basketId, BasketItem basketItem, int count)
               : base(basketId)
        {
            BasketItem = basketItem;
            Count = count;
        }

        public BasketItem BasketItem { get; }
        public int Count { get; }
    }
}
