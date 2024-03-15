using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Explorer.Encounters.Core.Mappers;
using Explorer.Encounters.Core.UseCases;
using Explorer.Encounters.Infrastructure.Database;
using Explorer.Encounters.Infrastructure.Database.Repositories;
using Explorer.Stakeholders.API.Internal;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.UseCases;
using Explorer.Stakeholders.Core.UseCases.Identity;
using Explorer.Stakeholders.Core.UseCases.Tourist;
using Explorer.Tours.API.Internal;
using Explorer.Tours.Core.UseCases.TourExecution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Explorer.Encounters.Infrastructure
{
    public static class EncountersStartup
    {
        public static IServiceCollection ConfigureEncountersModule(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(EncounterProfile).Assembly);
            SetupCore(services);
            SetupInfrastructure(services);
            return services;
        }

        private static void SetupCore(IServiceCollection services)
        {
            services.AddScoped<IEncounterService, EncounterService>();
            services.AddScoped<IEncounterCompletionService, EncounterCompletionService>();
            services.AddScoped<IKeypointEncounterService, KeypointEncounterService>();
            services.AddScoped<IInternalTouristPositionService, TouristPositionService>();
            services.AddScoped<IInternalUserService, UserService>();
            services.AddScoped<IInternalProfileService, ProfileService>();
            services.AddScoped<IXPService, XPService>();
            services.AddScoped<IInternalClubFightService, ClubFightService>();
            services.AddScoped<IInternalClubService, ClubService>();
            services.AddScoped<IStatisticsService, StatisticsService>();
        }

        private static void SetupInfrastructure(IServiceCollection services)
        {
            services.AddScoped(typeof(IEncounterRepository), typeof(EncounterRepository));
            services.AddScoped(typeof(IEncounterCompletionRepository), typeof(EncounterCompletionRepository));
            services.AddScoped(typeof(IKeypointEncounterRepository), typeof(KeypointEncounterRepository));

            services.AddDbContext<EncountersContext>(opt =>
                opt.UseNpgsql(DbConnectionStringBuilder.Build("encounters"),
                    x => x.MigrationsHistoryTable("__EFMigrationsHistory", "encounters")));
        }
    }
}
