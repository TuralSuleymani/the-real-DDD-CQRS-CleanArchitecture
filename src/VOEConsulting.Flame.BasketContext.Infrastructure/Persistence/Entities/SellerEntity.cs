namespace VOEConsulting.Flame.BasketContext.Infrastructure.Entities
{
    public class SellerEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public float Rating { get; set; }
        public decimal ShippingLimit { get; set; }
        public decimal ShippingCost { get; set; }
    }
}
