using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VOEConsulting.Flame.BasketContext.Infrastructure.Entities;

public class CustomerConfiguration : IEntityTypeConfiguration<CustomerEntity>
{
    public void Configure(EntityTypeBuilder<CustomerEntity> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.IsEliteMember)
            .IsRequired();
    }
}
