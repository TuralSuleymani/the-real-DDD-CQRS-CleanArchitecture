namespace VOEConsulting.Flame.BasketContext.Application.Baskets.Commands.AddItemToBasket
{
    public record AddItemToBasketCommand(Guid BasketId, SellerRequest Seller, BasketItemRequest BasketItem
        ,  QuantityRequest Quantity) : ICommand<Guid>
    {
    }

    public record QuantityRequest(int Value, int QuantityLimit, decimal PricePerUnit);
    public record SellerRequest(Guid Id, string Name, float Rating, decimal ShippingLimit, decimal ShippingCost);
    public record BasketItemRequest(Guid ItemId, string Name, string ImageUrl, int Limit); 

}
