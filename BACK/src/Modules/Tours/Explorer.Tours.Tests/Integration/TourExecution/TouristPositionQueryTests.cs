using Explorer.API.Controllers.Tourist.TourExecution;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourExecution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.TourExecution;

[Collection("Sequential")]
public class TouristPositionQueryTests : BaseToursIntegrationTest
{
    public TouristPositionQueryTests(ToursTestFactory factory) : base(factory)
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
        var result = ((ObjectResult)controller.GetByUser().Result)?.Value as TouristPositionDto;

        // Assert
        result.ShouldNotBeNull();
        result.UserId.ShouldBe(userId);
        result.Latitude.ShouldBe(20);
        result.Longitude.ShouldBe(30);
    }

    [Fact]
    public void Retrieves_fails()
    {
        // Arrange
        var userId = 10;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, userId);

        // Act
        var result = (ObjectResult)controller.GetByUser().Result;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    private static TouristPositionController CreateController(IServiceScope scope, int userId)
    {
        return new TouristPositionController(scope.ServiceProvider.GetRequiredService<ITouristPositionService>())
        {
            ControllerContext = BuildContext(userId.ToString())
        };
    }
}