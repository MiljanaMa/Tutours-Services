using Explorer.Tours.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Object = Explorer.Tours.Core.Domain.Object;

namespace Explorer.Tours.Infrastructure.Database;

public class ToursContext : DbContext
{
    public ToursContext(DbContextOptions<ToursContext> options) : base(options)
    {
    }

    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<TouristEquipment> TouristEquipment { get; set; }
    public DbSet<TourReview> TourReviews { get; set; }
    public DbSet<TourPreference> TourPreference { get; set; }
    public DbSet<Tour> Tours { get; set; }
    public DbSet<Keypoint> Keypoints { get; set; }
    public DbSet<Object> Objects { get; set; }
    public DbSet<TourEquipment> TourEquipments { get; set; }
    public DbSet<PublicEntityRequest> PublicEntityRequests { get; set; }
    public DbSet<TouristPosition> TouristPositions { get; set; }
    public DbSet<PublicKeypoint> PublicKeyPoints { get; set; }
    public DbSet<TourProgress> TourProgresses { get; set; }

    public DbSet<Bundle> Bundle {  get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("tours");

        ConfigureTour(modelBuilder);
        ConfigureTourProgress(modelBuilder);
    }

    private static void ConfigureTour(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Keypoint>()
            .HasOne(k => k.Tour)
            .WithMany(t => t.Keypoints)
            .HasForeignKey(k => k.TourId);

        modelBuilder.Entity<TourEquipment>()
            .HasKey(te => new { te.TourId, te.EquipmentId });

        modelBuilder.Entity<TourEquipment>()
            .HasOne<Tour>()
            .WithMany(t => t.TourEquipments)
            .HasForeignKey(te => te.TourId);

        modelBuilder.Entity<TourEquipment>()
            .HasOne<Equipment>()
            .WithMany(e => e.TourEquipments)
            .HasForeignKey(te => te.EquipmentId);

        modelBuilder.Entity<TourReview>()
            .HasOne(tr => tr.Tour)
            .WithMany(t => t.TourReviews)
            .HasForeignKey(tr => tr.TourId);

        modelBuilder.Entity<Tour>()
       .HasMany(t => t.Bundles)
       .WithMany(b => b.Tours);
    }

    private static void ConfigureTourProgress(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TourProgress>()
            .HasOne(tp => tp.TouristPosition)
            .WithMany()
            .HasForeignKey(tp => tp.TouristPositionId);

        modelBuilder.Entity<TourProgress>()
            .HasOne(tp => tp.Tour)
            .WithMany()
            .HasForeignKey(tp => tp.TourId);
    }
}