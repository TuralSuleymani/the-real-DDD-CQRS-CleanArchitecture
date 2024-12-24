using VOEConsulting.Flame.BasketContext.Domain.Baskets;
using VOEConsulting.Flame.Common.Core.Events;
using VOEConsulting.Flame.Common.Domain;

namespace VOEConsulting.Flame.BasketContext.Application.Events.Integration
{
    public sealed class BasketCreatedIntegrationEvent : IntegrationEvent
    {
        public BasketCreatedIntegrationEvent(Id<Basket> basketId, Guid customerId)
            : base(basketId)
        {
            CustomerId = customerId;
        }
        public BasketCreatedIntegrationEvent() { }

        public Guid CustomerId { get; set; }
    }
}
