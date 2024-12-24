using VOEConsulting.Flame.BasketContext.Domain.Baskets;

namespace VOEConsulting.Flame.BasketContext.Tests.Data
{
    public static class BasketData
    {
        public const decimal TaxAmount = 18;
        public static Customer Customer = Customer.Create(false, null);
    }
}
