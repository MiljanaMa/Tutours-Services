using Explorer.Encounters.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Encounters.Infrastructure.Database
{
    public class EncountersContext : DbContext
    {
        public DbSet<Encounter> Encounters { get; set; }
        public DbSet<EncounterCompletion> EncounterCompletions { get; set; }
        public DbSet<KeypointEncounter> KeypointEncounters { get; set; }

        public EncountersContext(DbContextOptions<EncountersContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("encounters");

            ConfigureEncounter(modelBuilder);
        }

        private static void ConfigureEncounter(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EncounterCompletion>()
                .HasOne(ec => ec.Encounter)
                .WithMany()
                .HasForeignKey(ec => ec.EncounterId);

            modelBuilder.Entity<KeypointEncounter>()
                .HasOne<Encounter>(ec => ec.Encounter)
                .WithOne()
                .HasForeignKey<KeypointEncounter>(s => s.EncounterId);
        }
    }
}
