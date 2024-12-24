using VOEConsulting.Flame.BasketContext.Tests.Data;
namespace VOEConsulting.Flame.BasketContext.Tests.Unit.Factories
{
    public static class QuantityFactory
    {
        public static Quantity Create(int? value = null, int? limit = null, decimal? pricePerUnit = null)
        {
            return Quantity.Create(value ?? BasketItemData.BasketItemQuantity.Value, limit ?? BasketItemData.BasketItemQuantity.Limit
                , pricePerUnit ?? BasketItemData.BasketItemQuantity.PricePerUnit);
        }
    }
}
