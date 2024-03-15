using Explorer.Stakeholders.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database;

public class StakeholdersContext : DbContext
{
    public StakeholdersContext(DbContextOptions<StakeholdersContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<ApplicationRating> ApplicationRatings { get; set; }
    public DbSet<ClubJoinRequest> ClubJoinRequests { get; set; }
    public DbSet<TourIssue> TourIssue { get; set; }
    public DbSet<TourIssueComment> TourIssueComments { get; set; }
    public DbSet<ClubInvitation> ClubInvitations { get; set; }
    public DbSet<Club> Clubs { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<ClubChallengeRequest> ClubChallengeRequests { get; set; }
    public DbSet<ClubFight> ClubFights { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<NewsletterPreference> NewsletterPreferences { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("stakeholders");

        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<TourIssue>().HasIndex(t => t.UserId).IsUnique(false);
        modelBuilder.Entity<TourIssueComment>().HasIndex(t => t.TourIssueId).IsUnique(false);
        ConfigureStakeholder(modelBuilder);
    }

    private static void ConfigureStakeholder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasOne<User>()
            .WithOne()
            .HasForeignKey<Person>(s => s.UserId);

        modelBuilder.Entity<Person>()
            .HasMany(u => u.Followers)
            .WithMany(u => u.Following);

        modelBuilder.Entity<Person>()
            .HasOne(p => p.Club)
            .WithMany(c => c.Members)
            .HasForeignKey(p => p.ClubId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<TourIssueComment>().HasOne<TourIssue>().WithMany(t => t.Comments)
            .HasForeignKey(te => te.TourIssueId);
        modelBuilder.Entity<TourIssueComment>().HasOne<User>().WithMany(u => u.IssueComments)
            .HasForeignKey(t => t.UserId);
        modelBuilder.Entity<TourIssue>().HasOne<User>().WithMany(u => u.Issues).HasForeignKey(t => t.UserId);

        modelBuilder.Entity<ChatMessage>()
            .HasOne(cm => cm.Sender)
            .WithMany()
            .HasForeignKey(cm => cm.SenderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ChatMessage>()
            .HasOne(cm => cm.Receiver)
            .WithMany()
            .HasForeignKey(cm => cm.ReceiverId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ChatMessage>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Club>()
            .HasOne(c => c.Owner)
            .WithMany()
            .HasForeignKey(c => c.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Club>()
            .HasMany(c => c.Achievements)
            .WithMany();

        modelBuilder.Entity<ClubChallengeRequest>()
            .HasOne(cc => cc.Challenger)
            .WithMany()
            .HasForeignKey(cc => cc.ChallengerId);
        
        modelBuilder.Entity<ClubChallengeRequest>()
            .HasOne(cc => cc.Challenged)
            .WithMany()
            .HasForeignKey(cc => cc.ChallengedId);

        modelBuilder.Entity<ClubFight>()
            .HasOne(cf => cf.Club1)
            .WithMany()
            .HasForeignKey(cf => cf.Club1Id);
        
        modelBuilder.Entity<ClubFight>()
            .HasOne(cf => cf.Club2)
            .WithMany()
            .HasForeignKey(cf => cf.Club2Id);
        
        modelBuilder.Entity<User>()
            .HasOne(u => u.NewsletterPreference) // User može da ima nula ili jedan NewsletterPreference
            .WithOne(np => np.User) // NewsletterPreference mora da ima tačno jedan User
            .HasForeignKey<NewsletterPreference>(np => np.UserID) // Spoljni ključ u NewsletterPreference koji pokazuje na User.Id
            .IsRequired(false); // NewsletterPreference.UserId nije obavezan (može biti NULL)

        modelBuilder.Entity<NewsletterPreference>()
            .HasOne(np => np.User) // NewsletterPreference pripada tačno jednom User-u
            .WithOne(u => u.NewsletterPreference) // Veza sa User entitetom
            .IsRequired(); // NewsletterPreference mora da ima User-a (ne može biti NULL)
    }
}