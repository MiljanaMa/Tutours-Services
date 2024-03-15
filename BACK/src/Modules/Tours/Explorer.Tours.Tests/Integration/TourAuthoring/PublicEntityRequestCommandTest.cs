using Explorer.API.Controllers.Author;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Dtos.Enums;
using Explorer.Tours.API.Public.TourAuthoring;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.TourAuthoring;

public class PublicEntityRequestCommandTest : BaseToursIntegrationTest
{
    public PublicEntityRequestCommandTest(ToursTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Creates_request_for_object()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var newEntity = new PublicEntityRequestDto
        {
            UserId = -11,
            EntityId = -3,
            EntityType = (EntityType)1, //object
            Status = 0,
            Comment = ""
        };

        // Act
        var result = ((ObjectResult)controller.CreateObjectRequest(newEntity).Result)?.Value as PublicEntityRequestDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.EntityId.ShouldBe(newEntity.EntityId);
        result.EntityType.ShouldBe(newEntity.EntityType);


        // Assert - Database
        var storedEntity = dbContext.PublicEntityRequests.FirstOrDefault(i =>
            i.EntityId == newEntity.EntityId && i.EntityType == Core.Domain.Enum.EntityType.OBJECT);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    [Fact]
    public void Creates_request_for_keypoint()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var newEntity = new PublicEntityRequestDto
        {
            UserId = -11,
            EntityId = -3,
            EntityType = 0, //keypoint
            Status = 0,
            Comment = ""
        };

        // Act
        var result =
            ((ObjectResult)controller.CreateKeypointRequest(newEntity).Result)?.Value as PublicEntityRequestDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.EntityId.ShouldBe(newEntity.EntityId);
        result.EntityType.ShouldBe(newEntity.EntityType);


        // Assert - Database
        var storedEntity = dbContext.PublicEntityRequests.FirstOrDefault(i =>
            i.EntityId == newEntity.EntityId && i.EntityType == Core.Domain.Enum.EntityType.KEYPOINT);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    public static PublicEntityRequestController CreateController(IServiceScope scope)
    {
        return new PublicEntityRequestController(
            scope.ServiceProvider.GetRequiredService<IPublicEntityRequestService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}