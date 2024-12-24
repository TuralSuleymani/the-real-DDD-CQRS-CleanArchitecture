namespace VOEConsulting.Flame.BasketContext.Infrastructure.Entities
{
    public class CustomerEntity
    {
        public Guid Id { get; set; }
        public bool IsEliteMember { get; set; }
        public ICollection<BasketEntity> Baskets { get; set; }
    }
}
