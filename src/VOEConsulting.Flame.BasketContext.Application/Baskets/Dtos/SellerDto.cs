namespace VOEConsulting.Flame.BasketContext.Application.Baskets.Dtos
{
    public class SellerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Rating { get; set; }
        public decimal ShippingLimit { get; set; }
        public decimal ShippingCost { get; set; }
    }



}
