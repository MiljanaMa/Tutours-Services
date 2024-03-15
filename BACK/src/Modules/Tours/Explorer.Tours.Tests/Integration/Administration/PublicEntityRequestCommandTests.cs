using Explorer.API.Controllers.Administrator;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Dtos.Enums;
using Explorer.Tours.API.Public.TourAuthoring;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using EntityType = Explorer.Tours.Core.Domain.Enum.EntityType;
using ObjectStatus = Explorer.Tours.Core.Domain.Enum.ObjectStatus;

namespace Explorer.Tours.Tests.Integration.Administration;

public class PublicEntityRequestCommandTest : BaseToursIntegrationTest
{
    public PublicEntityRequestCommandTest(ToursTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Approves_keypoint_request()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var acceptedRequest = new PublicEntityRequestDto
        {
            Id = 3,
            UserId = -12,
            EntityId = -2,
            EntityType = 0, //keypoint
            Status = 0,
            Comment = ""
        };

        // Act
        var result = ((ObjectResult)controller.Approve(acceptedRequest).Result)?.Value as PublicEntityRequestDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.EntityId.ShouldBe(acceptedRequest.EntityId);
        result.EntityType.ShouldBe(acceptedRequest.EntityType);
        result.Status.ShouldBe(PublicEntityRequestStatus.APPROVED);


        // Assert - Database
        var storedEntity = dbContext.PublicEntityRequests.FirstOrDefault(i =>
            i.EntityId == acceptedRequest.EntityId && i.EntityType == EntityType.KEYPOINT);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
        storedEntity.Status.ShouldBe(Core.Domain.Enum.PublicEntityRequestStatus.APPROVED);

        var keypointToPublic = dbContext.Keypoints.FirstOrDefault(k => k.Id == acceptedRequest.EntityId);

        var createdPublicKeypoint = dbContext.PublicKeyPoints.FirstOrDefault(kp =>
            kp.Name == keypointToPublic.Name && kp.Longitude == keypointToPublic.Longitude &&
            kp.Latitude == keypointToPublic.Latitude);
        createdPublicKeypoint.ShouldNotBeNull();
        createdPublicKeypoint.Name.ShouldBe(keypointToPublic.Name);
    }

    [Fact]
    public void Approves_object_request()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var acceptedRequest = new PublicEntityRequestDto
        {
            Id = 4,
            UserId = -12,
            EntityId = -2,
            EntityType = (API.Dtos.Enums.EntityType)1, //object
            Status = 0,
            Comment = ""
        };

        // Act
        var result = ((ObjectResult)controller.Approve(acceptedRequest).Result)?.Value as PublicEntityRequestDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.EntityId.ShouldBe(acceptedRequest.EntityId);
        result.EntityType.ShouldBe(acceptedRequest.EntityType);
        result.Status.ShouldBe(PublicEntityRequestStatus.APPROVED);


        // Assert - Database
        var storedEntity = dbContext.PublicEntityRequests.FirstOrDefault(i =>
            i.EntityId == acceptedRequest.EntityId && i.EntityType == EntityType.OBJECT);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
        storedEntity.Status.ShouldBe(Core.Domain.Enum.PublicEntityRequestStatus.APPROVED);

        var objectToPublic = dbContext.Objects.FirstOrDefault(o => o.Id == acceptedRequest.EntityId);
        objectToPublic.ShouldNotBeNull();
        objectToPublic.Status.ShouldBe(ObjectStatus.PUBLIC);
    }

    [Fact]
    public void Declines_request()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var declinedRequest = new PublicEntityRequestDto
        {
            Id = 2,
            UserId = -11,
            EntityId = -1,
            EntityType = (API.Dtos.Enums.EntityType)1, //object
            Status = 0,
            Comment = "Declined request for now."
        };

        // Act
        var result = ((ObjectResult)controller.Decline(declinedRequest).Result)?.Value as PublicEntityRequestDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.EntityId.ShouldBe(declinedRequest.EntityId);
        result.EntityType.ShouldBe(declinedRequest.EntityType);
        result.Status.ShouldBe(PublicEntityRequestStatus.DECLINED);


        // Assert - Database
        var storedEntity = dbContext.PublicEntityRequests.FirstOrDefault(i =>
            i.EntityId == declinedRequest.EntityId && i.EntityType == EntityType.OBJECT);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
        storedEntity.Status.ShouldBe(Core.Domain.Enum.PublicEntityRequestStatus.DECLINED);

        var objectToPublic = dbContext.Objects.FirstOrDefault(o => o.Id == declinedRequest.EntityId);
        objectToPublic.ShouldNotBeNull();
        objectToPublic.Status.ShouldBe(ObjectStatus.PRIVATE);
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