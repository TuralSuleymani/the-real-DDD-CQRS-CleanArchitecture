namespace VOEConsulting.Flame.BasketContext.Application.Baskets.Dtos
{
    public class BasketItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsActive { get; set; }
    }



}
