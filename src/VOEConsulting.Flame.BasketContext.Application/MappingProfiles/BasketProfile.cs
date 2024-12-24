using VOEConsulting.Flame.BasketContext.Domain.Baskets;

public class BasketMappingProfile : Profile
{
    public BasketMappingProfile()
    {
        // Map Basket to BasketDto
        CreateMap<Basket, BasketDto>()
            .ForMember(dest => dest.BasketItems, opt => opt.MapFrom(src =>
                src.BasketItems.ToDictionary(
                    kvp => MapSeller(kvp.Key),
                    kvp => new BasketItemInfoDto
                    {
                        Items = kvp.Value.Items.Select(MapBasketItem).ToList(),
                        ShippingAmountLeft = kvp.Value.ShippingAmountLeft
                    })))
            .ForMember(dest => dest.CouponId, opt => opt.MapFrom(src => src.CouponId));

        // Map Customer to CustomerDto
        CreateMap<Customer, CustomerDto>();

        // Map BasketItem to BasketItemDto
        CreateMap<BasketItem, BasketItemDto>()
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

        // Map Seller to SellerDto
        CreateMap<Seller, SellerDto>();
    }

    // Manual mappings for complex dictionary structures
    private static SellerDto MapSeller(Seller seller)
    {
        return new SellerDto
        {
            Id = seller.Id.Value,
            Name = seller.Name,
            Rating = seller.Rating,
            ShippingLimit = seller.ShippingLimit,
            ShippingCost = seller.ShippingCost
        };
    }

    private static BasketItemDto MapBasketItem(BasketItem basketItem)
    {
        return new BasketItemDto
        {
            Id = basketItem.Id.Value,
            Price = basketItem.Quantity.PricePerUnit,
            Quantity = basketItem.Quantity.Value,
            TotalPrice = basketItem.Quantity.TotalPrice,
            IsActive = basketItem.IsActive
        };
    }
}
