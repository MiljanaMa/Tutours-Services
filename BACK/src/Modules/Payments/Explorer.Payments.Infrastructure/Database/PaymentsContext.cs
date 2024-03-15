using Explorer.Payments.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Payments.Infrastructure.Database;

public class PaymentsContext : DbContext
{
    public PaymentsContext(DbContextOptions<PaymentsContext> options) : base(options)
    {
    }

    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<TourPurchaseToken> TourPurchaseTokens { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<PaymentRecord> PaymentRecords { get; set; }
    public DbSet<WishListItem> WishListItems { get; set; }
    public DbSet<WishList> WishLists { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<TourDiscount> TourDiscounts { get; set; }
    public DbSet<Coupon> Coupons { get; set; }

    public DbSet<BundlePrice> BundlePrices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("payments");

        modelBuilder.Entity<TourDiscount>()
            .HasKey(ts => new { ts.DiscountId, ts.TourId });

        modelBuilder.Entity<TourDiscount>()
            .HasOne<Discount>()
            .WithMany(e => e.TourDiscounts)
            .HasForeignKey(te => te.DiscountId);

        modelBuilder.Entity<Coupon>()
            .Property(e => e.ExpiryDate)
            .HasConversion(
                v => v.ToDateTime(TimeOnly.MinValue).Date,
                v => DateOnly.FromDateTime(v.Date))
            .HasColumnType("date");

    }
}