namespace VOEConsulting.Flame.BasketContext.Infrastructure.Entities
{
    public class CouponEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public decimal Value { get; set; }
        public string CouponType { get; set; } // "Fix" or "Percentage"
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<BasketEntity> Baskets { get; set; }
    }
}
