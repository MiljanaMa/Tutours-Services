using Explorer.BuildingBlocks.Tests;
using Explorer.Encounters.Infrastructure.Database;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Explorer.Encounters.Tests
{
    public class EncountersTestFactory : BaseTestFactory<EncountersContext>
    {
        protected override IServiceCollection ReplaceNeededDbContexts(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<EncountersContext>));
            services.Remove(descriptor!);
            services.AddDbContext<EncountersContext>(SetupTestContext());

            descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ToursContext>));
            services.Remove(descriptor!);
            services.AddDbContext<ToursContext>(SetupTestContext());

            descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<StakeholdersContext>));
            services.Remove(descriptor!);
            services.AddDbContext<StakeholdersContext>(SetupTestContext());

            return services;
        }
    }
}
