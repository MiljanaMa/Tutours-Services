using Explorer.API.Controllers.Tourist;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.MarketPlace;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.Marketplace;

[Collection("Sequential")]
public class TourPreferenceQueryTests : BaseToursIntegrationTest
{
    public TourPreferenceQueryTests(ToursTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Retrieves()
    {
        // Arrange
        var userId = -3;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, userId);

        // Act
        var result = ((ObjectResult)controller.GetByUser().Result)?.Value as TourPreferenceDto;

        // Assert
        result.ShouldNotBeNull();
        result.UserId.ShouldBe(userId);
        result.Difficulty.ShouldBe("MEDIUM");
        result.TransportType.ShouldBe("BIKE");
    }

    [Fact]
    public void Retrieves_fails()
    {
        // Arrange
        var userId = -10;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, userId);

        // Act
        var result = (ObjectResult)controller.GetByUser().Result;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    private static TourPreferenceController CreateController(IServiceScope scope, int userId)
    {
        return new TourPreferenceController(scope.ServiceProvider.GetRequiredService<ITourPreferenceService>())
        {
            ControllerContext = BuildContext(userId.ToString())
        };
    }
}