using Explorer.API.Controllers.Tourist;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.MarketPlace;
using Explorer.Tours.Core.Domain.Enum;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.Marketplace;

[Collection("Sequential")]
public class TourPreferenceCommandTests : BaseToursIntegrationTest
{
    public TourPreferenceCommandTests(ToursTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Creates()
    {
        // Arrange
        var userId = -2;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, userId);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var newEntity = new TourPreferenceDto
        {
            Difficulty = "EASY",
            TransportType = "WALK",
            Tags = new List<string> { "River Side" }
        };

        // Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TourPreferenceDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);

        // Assert - Database
        var storedEntity = dbContext.TourPreference.FirstOrDefault(tp => tp.UserId == userId);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    [Fact]
    public void Create_fails_preference_exists()
    {
        // Arrange
        var userId = -3;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, userId);
        var newEntity = new TourPreferenceDto
        {
            Difficulty = "EASY"
        };

        // Act
        var result = (ObjectResult)controller.Create(newEntity).Result;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(409);
    }

    [Fact]
    public void Updates()
    {
        // Arrange
        var userId = -3;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, userId);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var updatedEntity = new TourPreferenceDto
        {
            Difficulty = "EASY",
            TransportType = "WALK",
            Tags = new List<string> { "River Side" }
        };

        // Act
        var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as TourPreferenceDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.UserId.ShouldBe(updatedEntity.UserId);
        result.TransportType.ShouldBe(updatedEntity.TransportType);
        result.Difficulty.ShouldBe(updatedEntity.Difficulty);
        result.Tags.ShouldBe(updatedEntity.Tags);

        // Assert - Database
        var storedEntity = dbContext.TourPreference.FirstOrDefault(tp => tp.UserId == userId);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
        Enum.TryParse(updatedEntity.Difficulty, out TourDifficulty tourDifficulty);
        storedEntity.Difficulty.ShouldBe(tourDifficulty);
        Enum.TryParse(updatedEntity.TransportType, out TransportType transportType);
        storedEntity.TransportType.ShouldBe(transportType);
        storedEntity.Tags.ShouldBe(updatedEntity.Tags);
    }

    [Fact]
    public void Update_fails_preference_missing()
    {
        // Arrange
        var userId = -4;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, userId);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var updatedEntity = new TourPreferenceDto
        {
            Difficulty = "EASY",
            TransportType = "WALK",
            Tags = new List<string> { "River Side" }
        };

        // Act
        var result = (ObjectResult)controller.Update(updatedEntity).Result;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    [Fact]
    public void Deletes()
    {
        // Arrange
        var userId = -1;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, userId);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        // Act
        var result = (OkResult)controller.Delete();

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedCourse = dbContext.TourPreference.FirstOrDefault(tp => tp.Id == userId);
        storedCourse.ShouldBeNull();
    }

    private static TourPreferenceController CreateController(IServiceScope scope, int userId)
    {
        return new TourPreferenceController(scope.ServiceProvider.GetRequiredService<ITourPreferenceService>())
        {
            ControllerContext = BuildContext(userId.ToString())
        };
    }
}