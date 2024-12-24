using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOEConsulting.Flame.BasketContext.Infrastructure.Entities;

namespace VOEConsulting.Flame.BasketContext.Infrastructure.Persistence.Configurations
{
    public class SellerEntityConfiguration : IEntityTypeConfiguration<SellerEntity>
    {
        public void Configure(EntityTypeBuilder<SellerEntity> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.Rating)
                .IsRequired();

            builder.Property(s => s.ShippingLimit)
                .IsRequired();

            builder.Property(s => s.ShippingCost)
                .IsRequired();
        }
    }

}
