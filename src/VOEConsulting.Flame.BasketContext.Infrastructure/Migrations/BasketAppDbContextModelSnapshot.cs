﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VOEConsulting.Infrastructure.Persistence;

#nullable disable

namespace VOEConsulting.Flame.BasketContext.Infrastructure.Migrations
{
    [DbContext(typeof(BasketAppDbContext))]
    partial class BasketAppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("VOEConsulting.Flame.BasketContext.Infrastructure.Entities.BasketEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CouponId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("TaxPercentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalAmount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(18,2)")
                        .HasDefaultValue(0m);

                    b.HasKey("Id");

                    b.HasIndex("CouponId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Baskets", (string)null);
                });

            modelBuilder.Entity("VOEConsulting.Flame.BasketContext.Infrastructure.Entities.BasketItemEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BasketId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<decimal>("PricePerUnit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("QuantityLimit")
                        .HasColumnType("int");

                    b.Property<int>("QuantityValue")
                        .HasColumnType("int");

                    b.Property<Guid>("SellerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BasketId");

                    b.HasIndex("SellerId");

                    b.ToTable("BasketItems");
                });

            modelBuilder.Entity("VOEConsulting.Flame.BasketContext.Infrastructure.Entities.CouponEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("CouponType")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Coupons", (string)null);
                });

            modelBuilder.Entity("VOEConsulting.Flame.BasketContext.Infrastructure.Entities.CustomerEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsEliteMember")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("VOEConsulting.Flame.BasketContext.Infrastructure.Entities.SellerEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<float>("Rating")
                        .HasColumnType("real");

                    b.Property<decimal>("ShippingCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ShippingLimit")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Sellers");
                });

            modelBuilder.Entity("VOEConsulting.Flame.BasketContext.Infrastructure.Entities.BasketEntity", b =>
                {
                    b.HasOne("VOEConsulting.Flame.BasketContext.Infrastructure.Entities.CouponEntity", "Coupon")
                        .WithMany("Baskets")
                        .HasForeignKey("CouponId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("VOEConsulting.Flame.BasketContext.Infrastructure.Entities.CustomerEntity", "Customer")
                        .WithMany("Baskets")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coupon");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("VOEConsulting.Flame.BasketContext.Infrastructure.Entities.BasketItemEntity", b =>
                {
                    b.HasOne("VOEConsulting.Flame.BasketContext.Infrastructure.Entities.BasketEntity", "Basket")
                        .WithMany("BasketItems")
                        .HasForeignKey("BasketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VOEConsulting.Flame.BasketContext.Infrastructure.Entities.SellerEntity", "Seller")
                        .WithMany()
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Basket");

                    b.Navigation("Seller");
                });

            modelBuilder.Entity("VOEConsulting.Flame.BasketContext.Infrastructure.Entities.BasketEntity", b =>
                {
                    b.Navigation("BasketItems");
                });

            modelBuilder.Entity("VOEConsulting.Flame.BasketContext.Infrastructure.Entities.CouponEntity", b =>
                {
                    b.Navigation("Baskets");
                });

            modelBuilder.Entity("VOEConsulting.Flame.BasketContext.Infrastructure.Entities.CustomerEntity", b =>
                {
                    b.Navigation("Baskets");
                });
#pragma warning restore 612, 618
        }
    }
}
