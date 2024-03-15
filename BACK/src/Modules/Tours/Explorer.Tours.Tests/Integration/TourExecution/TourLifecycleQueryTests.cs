using Explorer.API.Controllers.Tourist.TourExecution;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourExecution;
using Explorer.Tours.Core.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.TourExecution;

[Collection("Sequential")]
public class TourLifecycleQueryTests : BaseToursIntegrationTest
{
    public TourLifecycleQueryTests(ToursTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Retrieves()
    {
        // Arrange
        var userId = -1;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, userId);

        // Act
        var result = ((ObjectResult)controller.GetActiveByUser().Result)?.Value as TourProgressDto;

        // Assert
        result.ShouldNotBeNull();
        result.CurrentKeyPoint.ShouldBe(3);
        result.Status.ShouldBe(TourProgressStatus.IN_PROGRESS.ToString());
        result.TouristPosition.UserId.ShouldBe(userId);
    }

    [Fact]
    public void Retrieves_fails_no_active_tour()
    {
        // Arrange
        var userId = -2;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, userId);

        // Act
        var result = (ObjectResult)controller.GetActiveByUser().Result;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    private static TourLifecycleController CreateController(IServiceScope scope, int userId)
    {
        return new TourLifecycleController(scope.ServiceProvider.GetRequiredService<ITourLifecycleService>())
        {
            ControllerContext = BuildContext(userId.ToString())
        };
    }
}