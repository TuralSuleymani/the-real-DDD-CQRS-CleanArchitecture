using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VOEConsulting.Flame.BasketContext.Infrastructure.Entities;

public class BasketConfiguration : IEntityTypeConfiguration<BasketEntity>
{
    public void Configure(EntityTypeBuilder<BasketEntity> builder)
    {
        builder.ToTable("Baskets");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.TaxPercentage)
            .IsRequired();

        builder.Property(b => b.TotalAmount)
            .HasDefaultValue(0);

        builder.HasOne(b => b.Customer)
            .WithMany(c => c.Baskets)
            .HasForeignKey(b => b.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Coupon)
            .WithMany(c => c.Baskets)
            .HasForeignKey(b => b.CouponId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(b => b.BasketItems)
            .WithOne(bi => bi.Basket)
            .HasForeignKey(bi => bi.BasketId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
