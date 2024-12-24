namespace VOEConsulting.Flame.BasketContext.Domain.Baskets
{
    public sealed class Customer : Entity<Customer>
    {
        public bool IsEliteMember { get; }

        private Customer(bool isEliteMember, Id<Customer>? id) 
            : base(id ?? Id<Customer>.FromString("00000000-0000-0000-0000-000000000001"))
        {
            IsEliteMember = isEliteMember;
        }

        public decimal DiscountPercentage
        {
            get
            {
                return IsEliteMember ? 0.1m : 0;
            }
        }

        public static Customer Create(bool isEliteMember, Id<Customer>? id = null)
        {
            return new Customer(isEliteMember, id);
        }
    }
}
