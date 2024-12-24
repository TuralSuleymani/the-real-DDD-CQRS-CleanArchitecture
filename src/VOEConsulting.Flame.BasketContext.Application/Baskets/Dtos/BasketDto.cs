namespace VOEConsulting.Flame.BasketContext.Application.Baskets.Dtos
{
    public class BasketDto
    {
        public Guid Id { get; set; }
        public IDictionary<SellerDto, BasketItemInfoDto> BasketItems { get; set; } = new Dictionary<SellerDto, BasketItemInfoDto>();
        public decimal TaxPercentage { get; set; }
        public decimal TotalAmount { get; set; }
        public CustomerDto Customer { get; set; }
        public Guid? CouponId { get; set; }
    }
}
