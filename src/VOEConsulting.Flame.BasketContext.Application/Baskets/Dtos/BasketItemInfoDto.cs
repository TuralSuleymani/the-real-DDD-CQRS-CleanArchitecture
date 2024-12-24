namespace VOEConsulting.Flame.BasketContext.Application.Baskets.Dtos
{
    public class BasketItemInfoDto
    {
        public IList<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();
        public decimal ShippingAmountLeft { get; set; }
    }



}
