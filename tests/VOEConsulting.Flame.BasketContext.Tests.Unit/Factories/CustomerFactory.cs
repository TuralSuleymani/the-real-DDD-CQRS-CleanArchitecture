using VOEConsulting.Flame.Common.Domain;

namespace VOEConsulting.Flame.BasketContext.Tests.Unit.Factories
{
    public static class CustomerFactory
    {
        public static Customer Create(bool? isEliteMember = null, Id<Customer>? id = null)
        {
            return Customer.Create(isEliteMember ?? false, id ?? Id<Customer>.New());
        }
    }
}
