using System.Text.Json.Serialization;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.API.Internal;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.API.Public.Identity;
using Explorer.Stakeholders.API.Public.TourExecution;
using Explorer.Stakeholders.API.Public.Tourist;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Mappers;
using Explorer.Stakeholders.Core.UseCases;
using Explorer.Stakeholders.Core.UseCases.Identity;
using Explorer.Stakeholders.Core.UseCases.TourExecution;
using Explorer.Stakeholders.Core.UseCases.Tourist;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Stakeholders.Infrastructure.Database.Repositories;
using Explorer.Stakeholders.API.Public.TourExecution;
using Explorer.Stakeholders.Core.UseCases.TourExecution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Explorer.Tours.API.Internal;
using Explorer.Tours.Core.UseCases.TourAuthoring;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Explorer.Stakeholders.Infrastructure.Database.NewFolder;

namespace Explorer.Stakeholders.Infrastructure;

public static class StakeholdersStartup
{
    public static IServiceCollection ConfigureStakeholdersModule(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(StakeholderProfile).Assembly);
        SetupCore(services);
        SetupInfrastructure(services);
        return services;
    }

    private static void SetupCore(IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ITokenGenerator, JwtGenerator>();
        services.AddScoped<IClubJoinRequestService, ClubJoinRequestService>();
        services.AddScoped<IClubService, ClubService>();
        services.AddScoped<IClubInvitationService, ClubInvitationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IInternalUserService, UserService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IInternalProfileService, ProfileService>();
        services.AddScoped<IApplicationRatingService, ApplicationRatingService>();
        services.AddScoped<ITourIssueService, TourIssueService>();
        services.AddScoped<ITourIssueCommentService, TourIssueCommentService>();
        services.AddScoped<IChatMessageService, ChatMessageService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IInternalTourService, TourService>();
        services.AddScoped<IInternalNotificationService, NotificationService>();
        services.AddScoped<IClubFightService, ClubFightService>();
        services.AddScoped<IClubChallengeRequestService, ClubChallengeRequestService>();
        services.AddScoped<IAchievementService, AchievementService>();
        services.AddScoped<IInternalAchievementService, AchievementService>();
        services.AddScoped<IInternalClubService, ClubService>();
        services.AddScoped<INewsletterPreferenceService, NewsletterPreferenceService>();
        services.AddScoped<IInternalEmailService, EmailService>();
        services.AddScoped<IEmailVerificationService, EmailService>();
    }

    private static void SetupInfrastructure(IServiceCollection services)
    {
        services.AddHostedService<NewsletterBackgroundService>();
        services.AddScoped(typeof(ICrudRepository<Person>),
            typeof(CrudDatabaseRepository<Person, StakeholdersContext>));
        services.AddScoped(typeof(ICrudRepository<ClubJoinRequest>),
            typeof(CrudDatabaseRepository<ClubJoinRequest, StakeholdersContext>));
        services.AddScoped(typeof(IUserRepository), typeof(UserDatabaseRepository));
        services.AddScoped(typeof(IClubJoinRequestRepository), typeof(ClubJoinRequestDatabaseRepository));
        services.AddScoped(typeof(ICrudRepository<User>), typeof(CrudDatabaseRepository<User, StakeholdersContext>));
        services.AddScoped(typeof(IClubRepository), typeof(ClubRepository));
        services.AddScoped(typeof(ICrudRepository<ClubInvitation>),
            typeof(CrudDatabaseRepository<ClubInvitation, StakeholdersContext>));
        services.AddScoped(typeof(ICrudRepository<TourIssue>),
            typeof(CrudDatabaseRepository<TourIssue, StakeholdersContext>));
        services.AddScoped(typeof(IPersonRepository), typeof(PersonDatabaseRepository));
        services.AddScoped(typeof(ITourIssueRepository), typeof(TourIssueDatabaseRepository));
        services.AddScoped(typeof(ICrudRepository<TourIssueComment>),
            typeof(CrudDatabaseRepository<TourIssueComment, StakeholdersContext>));
        services.AddScoped(typeof(IApplicationRatingRepository), typeof(ApplicationRatingDatabaseRepository));
        services.AddScoped(typeof(IChatMessageRepository), typeof(ChatMessageDatabaseRepository));
        services.AddScoped(typeof(INotificationRepository), typeof(NotificationRepository));
        services.AddScoped(typeof(IClubChallengeRequestRepository), typeof(ClubChallengeRequestRepository));
        services.AddScoped(typeof(IClubFightRepository), typeof(ClubFightRepository));
        services.AddScoped(typeof(IAchievementRepository), typeof(AchievementDatabaseRepository));
        services.AddScoped(typeof(ICrudRepository<NewsletterPreference>), typeof(CrudDatabaseRepository<NewsletterPreference, StakeholdersContext>));

        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.WriteIndented = true;
        });

        services.AddDbContext<StakeholdersContext>(opt =>
            opt.UseNpgsql(DbConnectionStringBuilder.Build("stakeholders"),
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "stakeholders")));
    }
}