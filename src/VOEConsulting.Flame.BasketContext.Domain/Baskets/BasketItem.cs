namespace VOEConsulting.Flame.BasketContext.Domain.Baskets
{
    public sealed class BasketItem : Entity<BasketItem>
    {
        public string Name { get; }
        public string ImageUrl { get; }

        public const int MinItemCount = 1;
        public Quantity Quantity { get; private set; }
        public Seller Seller { get; }
        public bool IsActive { get; private set; }

        private BasketItem(string name, Quantity quantity, string imageUrl, Seller seller, Id<BasketItem>? id)
            : base(id ?? Id<BasketItem>.New())
        {
            Name = name.EnsureNonBlank();
            ImageUrl = imageUrl.EnsureImageUrl();
            Quantity = quantity;
            Seller = seller;
            IsActive = true;
        }

        public static BasketItem Create(string name, Quantity quantity, string imageUrl, Seller seller, Id<BasketItem>? id = null)
        {
            return new BasketItem(name, quantity,imageUrl, seller, id);
        }

        public void UpdateCount(int basketItemCount)
        {
            basketItemCount.EnsureGreaterThan(MinItemCount);
            Quantity = Quantity.UpdateValue(basketItemCount);
        }
        public void Deactivate()
        {
            IsActive = false;
        }
        public void Activate()
        {
            IsActive = true;
        }
    }
}
