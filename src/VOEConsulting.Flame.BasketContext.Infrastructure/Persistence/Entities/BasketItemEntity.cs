namespace VOEConsulting.Flame.BasketContext.Infrastructure.Entities
{
    public class BasketItemEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public int QuantityValue { get; set; }
        public int QuantityLimit { get; set; }
        public decimal PricePerUnit { get; set; }
        public Guid SellerId { get; set; }
        public Guid BasketId { get; set; }
        public SellerEntity Seller { get; set; } = null!;
        public BasketEntity Basket { get; set; } = null!;
    }
}
