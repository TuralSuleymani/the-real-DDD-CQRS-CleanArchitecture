using VOEConsulting.Flame.BasketContext.Tests.Data;
using VOEConsulting.Flame.Common.Domain;
namespace VOEConsulting.Flame.BasketContext.Tests.Unit.Factories
{
    public static class SellerFactory
    {
        public static Seller Create(Id<Seller>? sellerId = null, string? name = null, float? rating = null,
            decimal? shippingLimit = null, decimal? shippingCost = null)
        {
            return Seller.Create(name ?? BasketItemData.Seller.Name, rating ?? BasketItemData.Seller.Rating
                , shippingLimit ?? BasketItemData.Seller.ShippingLimit, shippingCost ?? BasketItemData.Seller.ShippingCost,
                sellerId ?? BasketItemData.Seller.Id);
        }
    }
}
