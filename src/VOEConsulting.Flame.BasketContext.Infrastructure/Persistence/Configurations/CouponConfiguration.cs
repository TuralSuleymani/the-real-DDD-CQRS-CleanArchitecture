using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VOEConsulting.Flame.BasketContext.Infrastructure.Entities;
using static Azure.Core.HttpHeader;

public class CouponConfiguration : IEntityTypeConfiguration<CouponEntity>
{
    public void Configure(EntityTypeBuilder<CouponEntity> builder)
    {
        builder.ToTable("Coupons");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(c => c.IsActive)
            .IsRequired();

        builder.Property(c => c.Value)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(c => c.CouponType)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.StartDate)
            .IsRequired();

        builder.Property(c => c.EndDate)
            .IsRequired();
    }
}
