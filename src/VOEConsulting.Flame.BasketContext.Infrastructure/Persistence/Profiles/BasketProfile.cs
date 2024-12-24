using AutoMapper;
using VOEConsulting.Flame.BasketContext.Domain.Baskets;
using VOEConsulting.Flame.BasketContext.Infrastructure.Entities;

public class BasketMappingProfile : Profile
{
    public BasketMappingProfile()
    {
        // Domain -> Entity mappings
        CreateMap<Basket, BasketEntity>()
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Customer.Id))
            .ForMember(dest => dest.BasketItems, opt => opt.MapFrom(src => src.BasketItems.SelectMany(kvp => kvp.Value.Items)));

        CreateMap<BasketItem, BasketItemEntity>()
             .ForMember(dest => dest.PricePerUnit, opt => opt.MapFrom(src => src.Quantity.PricePerUnit))
            .ForMember(dest => dest.SellerId, opt => opt.MapFrom(src => src.Seller.Id))
            .ForMember(dest => dest.Seller, opt => opt.MapFrom(src => src.Seller)); // Avoid circular mapping

        CreateMap<Seller, SellerEntity>();
        CreateMap<Customer, CustomerEntity>();

        // Entity -> Domain mappings
        CreateMap<BasketEntity, Basket>()
            .ConstructUsing((entity, context) =>
            {
                var customer = context.Mapper.Map<Customer>(entity.Customer);
                return Basket.Create(entity.TaxPercentage, customer);
            })
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.BasketItems, opt => opt.Ignore()) // Manual mapping for complex structure
            .AfterMap((entity, domain, context) =>
            {
                if (entity.BasketItems != null)
                {
                    foreach (var itemEntity in entity.BasketItems)
                    {
                        var seller = context.Mapper.Map<Seller>(itemEntity.Seller);
                        var basketItem = context.Mapper.Map<BasketItem>(itemEntity);
                        domain.AddItem(basketItem);
                    }
                }
            });

        CreateMap<BasketItemEntity, BasketItem>()
            .ConstructUsing((entity, context) =>
            {
                var seller = context.Mapper.Map<Seller>(entity.Seller);
                var quantity = Quantity.Create(entity.QuantityValue, entity.QuantityLimit, entity.PricePerUnit);
                return BasketItem.Create(entity.Name, quantity, entity.ImageUrl, seller, entity.Id);
            });

        CreateMap<SellerEntity, Seller>()
            .ConstructUsing(entity => Seller.Create(entity.Name, entity.Rating, entity.ShippingLimit, entity.ShippingCost, entity.Id));

        CreateMap<CustomerEntity, Customer>()
            .ConstructUsing(entity => Customer.Create(entity.IsEliteMember, entity.Id));
    }

}
