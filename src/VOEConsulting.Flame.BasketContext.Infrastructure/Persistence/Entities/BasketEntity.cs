using System.Collections.Generic;

namespace VOEConsulting.Flame.BasketContext.Infrastructure.Entities
{
    public class BasketEntity
    {
        public Guid Id { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid CustomerId { get; set; }
        public CustomerEntity Customer { get; set; }
        public Guid? CouponId { get; set; }
        public CouponEntity Coupon { get; set; }
        public ICollection<BasketItemEntity> BasketItems { get; set; }
    }

}
