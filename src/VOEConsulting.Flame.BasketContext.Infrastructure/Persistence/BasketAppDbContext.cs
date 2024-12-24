using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VOEConsulting.Flame.BasketContext.Infrastructure.Entities;

namespace VOEConsulting.Infrastructure.Persistence;

public class BasketAppDbContext : DbContext
{
    public BasketAppDbContext(DbContextOptions<BasketAppDbContext> options)
        : base(options) { }

    public DbSet<BasketEntity> Baskets { get; set; }
    public DbSet<BasketItemEntity> BasketItems { get; set; }
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<SellerEntity> Sellers { get; set; }
    public DbSet<CouponEntity> Coupons { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply Fluent API configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Optional: Add additional configurations if required

        base.OnModelCreating(modelBuilder);
    }

}
