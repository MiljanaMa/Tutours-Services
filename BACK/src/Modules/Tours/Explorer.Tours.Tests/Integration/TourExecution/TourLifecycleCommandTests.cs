using Explorer.API.Controllers.Tourist.TourExecution;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourExecution;
using Explorer.Tours.Core.Domain.Enum;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.TourExecution;

[Collection("Sequential")]
public class TourLifecycleCommandTests : BaseToursIntegrationTest
{
    public TourLifecycleCommandTests(ToursTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Start()
    {
        // Arrange
        var userId = -3;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, userId);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var tourId = -2;

        // Act
        var result = ((ObjectResult)controller.StartTour(tourId).Result)?.Value as TourProgressDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Status.ShouldBe(TourProgressStatus.IN_PROGRESS.ToString());
        result.CurrentKeyPoint.ShouldBe(1);

        // Assert - Database
        var storedEntity = dbContext.TourProgresses.FirstOrDefault(tp => tp.Id == result.Id);
        storedEntity.ShouldNotBeNull();
        storedEntity.Status.ShouldBeEquivalentTo(TourProgressStatus.IN_PROGRESS);
        storedEntity.CurrentKeyPoint.ShouldBe(1);
    }

    [Fact]
    public void Abandon_active_tour()
    {
        // Arrange
        var userId = -1;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, userId);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        // Act
        var result = ((ObjectResult)controller.AbandonActiveTour().Result)?.Value as TourProgressDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-2);
        result.Status.ShouldBe(TourProgressStatus.ABANDONED.ToString());
        result.CurrentKeyPoint.ShouldBe(3);

        // Assert - Database
        var storedEntity = dbContext.TourProgresses.FirstOrDefault(tp => tp.Id == result.Id);
        storedEntity.ShouldNotBeNull();
        storedEntity.Status.ShouldBeEquivalentTo(TourProgressStatus.ABANDONED);
        storedEntity.CurrentKeyPoint.ShouldBe(3);
    }

    [Fact]
    public void Abandon_fails_no_active_tour()
    {
        // Arrange
        var userId = -2;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, userId);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        // Act
        var result = (ObjectResult)controller.AbandonActiveTour().Result;

        // Assert - Response
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