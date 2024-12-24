using VOEConsulting.Flame.BasketContext.Tests.Data;
using VOEConsulting.Flame.Common.Domain;
using static VOEConsulting.Flame.BasketContext.Tests.Data.BasketItemData;
namespace VOEConsulting.Flame.BasketContext.Tests.Unit.Factories
{
    public static class BasketItemFactory
    {
        public static BasketItem Create(string? name = null, Quantity? quantity = null, string? imageUrl = null,
            Seller? seller = null, Id<BasketItem>? id = null)
        {
            return BasketItem.Create(name?? BasketItemName, quantity?? BasketItemQuantity, imageUrl?? BasketItemImageUrl,
                seller?? SellerFactory.Create(), id?? BasketItemId);
        }
    }
}
