namespace VOEConsulting.Flame.BasketContext.Domain.Baskets.Events
{
    public sealed class CustomerAssignedEvent : BaseBasketDomainEvent
    {
        public CustomerAssignedEvent(Id<Basket> basketId, Customer customer)
            : base(basketId)
        {
            Customer = customer;
        }
        public Customer Customer { get; }
    }
}