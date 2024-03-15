using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.API.Public;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.API.Public.MarketPlace;
using Explorer.Tours.API.Public.Statistics;
using Explorer.Tours.API.Public.TourAuthoring;
using Explorer.Tours.API.Public.TourExecution;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Mappers;
using Explorer.Tours.Core.UseCases;
using Explorer.Tours.Core.UseCases.Administration;
using Explorer.Tours.Core.UseCases.MarketPlace;
using Explorer.Tours.Core.UseCases.Statistics;
using Explorer.Tours.Core.UseCases.TourAuthoring;
using Explorer.Tours.Core.UseCases.TourExecution;
using Explorer.Tours.Infrastructure.Database;
using Explorer.Tours.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Explorer.Tours.Infrastructure;

public static class ToursStartup
{
    public static IServiceCollection ConfigureToursModule(this IServiceCollection services)
    {
        // Registers all profiles since it works on the assembly
        services.AddAutoMapper(typeof(ToursProfile).Assembly);
        SetupCore(services);
        SetupInfrastructure(services);
        return services;
    }

    private static void SetupCore(IServiceCollection services)
    {
        services.AddScoped<IEquipmentService, EquipmentService>();
        services.AddScoped<ITouristEquipmentService, TouristEquipmentService>();
        services.AddScoped<ITourReviewService, TourReviewService>();
        services.AddScoped<ITourPreferenceService, TourPreferenceService>();
        services.AddScoped<ITourService, TourService>();
        services.AddScoped<IKeypointService, KeypointService>();
        services.AddScoped<IObjectService, ObjectService>();
        services.AddScoped<ITourEquipmentService, TourEquipmentService>();
        services.AddScoped<ITouristPositionService, TouristPositionService>();
        services.AddScoped<IPublicEntityRequestService, PublicEntityRequestService>();
        services.AddScoped<IPublicKeypointService, PublicKeypointService>();
        services.AddScoped<ITourLifecycleService, TourLifecycleService>();
        services.AddScoped<ITourFilteringService, TourFilteringService>();
        services.AddScoped<IStatisticsService, StatisticsService>();
        services.AddScoped<IBundleService, BundleService>();

    }

    private static void SetupInfrastructure(IServiceCollection services)
    {
        services.AddScoped(typeof(ICrudRepository<Equipment>), typeof(CrudDatabaseRepository<Equipment, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<TouristEquipment>),
            typeof(CrudDatabaseRepository<TouristEquipment, ToursContext>));
        services.AddScoped<ITouristEquipmentRepository, TouristEquipmentRepository>();
        services.AddScoped<IEquipmentRepository, EquipmentRepository>();
        services.AddScoped(typeof(ICrudRepository<TourReview>),
            typeof(CrudDatabaseRepository<TourReview, ToursContext>));
        services.AddScoped(typeof(ITourPreferenceRepository), typeof(TourPreferenceRepository));
        services.AddScoped(typeof(IKeypointRepository), typeof(KeypointRepository));
        services.AddScoped(typeof(ITourRepository), typeof(TourRepository));
        services.AddScoped(typeof(ICrudRepository<Keypoint>), typeof(CrudDatabaseRepository<Keypoint, ToursContext>));
        // services.AddScoped(typeof(ICrudRepository<Object>), typeof(CrudDatabaseRepository<Object, ToursContext>));
        services.AddScoped(typeof(ITourEquipmentRepository), typeof(TourEquipmentDatabaseRepository));
        services.AddScoped(typeof(ITouristPositionRepository), typeof(TouristPositionRepository));
        services.AddScoped(typeof(IPublicEntityRequestRepository), typeof(PublicEntityRequestRepository));
        services.AddScoped(typeof(IPublicKeypointRepository), typeof(PublicKeypointRepository));
        services.AddScoped(typeof(IObjectRepository), typeof(ObjectRepository));
        services.AddScoped(typeof(ITourProgressRepository), typeof(TourProgressRepository));
        services.AddScoped(typeof(ITourReviewRepository), typeof(TourReviewRepository));
        services.AddScoped ( typeof(IBundleRepository), typeof(BundleRepository));

        services.AddDbContext<ToursContext>(opt =>
            opt.UseNpgsql(DbConnectionStringBuilder.Build("tours"),
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "tours")));
    }
}