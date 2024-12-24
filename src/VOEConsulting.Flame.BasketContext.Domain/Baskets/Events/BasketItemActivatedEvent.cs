namespace VOEConsulting.Flame.BasketContext.Domain.Baskets.Events
{
    public sealed class BasketItemActivatedEvent : BaseBasketDomainEvent 
    {
        public BasketItemActivatedEvent(Id<Basket> basketId, BasketItem basketItem)
               : base(basketId)
        {
            BasketItem = basketItem;
        }

        public BasketItem BasketItem { get; }
    }
}