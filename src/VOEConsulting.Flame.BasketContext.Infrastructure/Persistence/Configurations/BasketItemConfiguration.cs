using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VOEConsulting.Flame.BasketContext.Infrastructure.Entities;

public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItemEntity>
{
    public void Configure(EntityTypeBuilder<BasketItemEntity> builder)
    {
        builder.HasKey(bi => bi.Id);

        builder.Property(bi => bi.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(bi => bi.ImageUrl)
            .IsRequired();

        builder.Property(bi => bi.QuantityValue)
            .IsRequired();

        builder.Property(bi => bi.QuantityLimit)
            .IsRequired();

        builder.Property(bi => bi.PricePerUnit)
            .IsRequired();

        builder.HasOne(bi => bi.Seller)
            .WithMany()
            .HasForeignKey(bi => bi.SellerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
