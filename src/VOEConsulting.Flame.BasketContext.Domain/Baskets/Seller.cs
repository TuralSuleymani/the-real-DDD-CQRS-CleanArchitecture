using VOEConsulting.Flame.BasketContext.Domain.Baskets.Services;

namespace VOEConsulting.Flame.BasketContext.Domain.Baskets
{
    public sealed class Seller : Entity<Seller>
    {
        private Seller(Id<Seller> sellerId, string name, float rating, decimal shippingLimit, decimal shippingCost)
            : base(sellerId)
        {
            Name = name;
            Rating = rating;
            ShippingLimit = shippingLimit;
            ShippingCost = shippingCost;
        }

        public static Seller Create(string name, float rating, decimal shippingLimit, decimal shippingCost, Id<Seller>? sellerId)
        {
            return new Seller(sellerId ?? Id<Seller>.New(), name, rating, shippingLimit, shippingCost);
        }

        public string Name { get; }
        public float Rating { get; }
        public decimal ShippingLimit { get; }
        public decimal ShippingCost { get; }

        public int GetLimitForProduct(string productName, ISellerLimitService limitService)
        {
            // Delegate the logic to a domain or application service
            return limitService.GetLimitForProduct(Id, productName);
        }

    }
}
