using VOEConsulting.Flame.BasketContext.Domain.Baskets;
using VOEConsulting.Flame.Common.Domain;

namespace VOEConsulting.Flame.BasketContext.Tests.Data
{
    public static class BasketItemData
    {
        public const int BasketItemLimit = 10;
        public static Id<BasketItem> BasketItemId = Id<BasketItem>.New();
        public const string BasketItemName = "Test Basket Item";
        public const string BasketItemImageUrl = "https://test.com/image.jpg";
        public static Quantity BasketItemQuantity = Quantity.Create(1, BasketItemLimit, 40);
        public const decimal BasketItemPrice = 130.0m;
        public static Seller Seller = Seller.Create("Test Seller", 8.8f, 200, 39.9m, Id<Seller>.New());
    }
}
