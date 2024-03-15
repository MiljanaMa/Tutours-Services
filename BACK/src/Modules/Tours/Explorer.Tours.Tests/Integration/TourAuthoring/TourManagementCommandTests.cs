using Explorer.API.Controllers.Author;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourAuthoring;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.TourAuthoring;

public class TourManagementCommandTests : BaseToursIntegrationTest
{
    
    public TourManagementCommandTests(ToursTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Creates()
    {
        // Arrange
        var userId = -1;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, userId);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var newEntity = new TourDto()
        {
            UserId = 1,
            Name = "Test",
            Description = "Test",
            Price = 0,
            Duration = 100,
            Distance = 2.1,
            Difficulty = "EASY",
            TransportType = "CAR",
            Status = "DRAFT",
            Tags = new(),
            StatusUpdateTime = DateTime.Now.ToUniversalTime()
        };

        // Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TourDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Name.ShouldBe(newEntity.Name);

        // Assert - Database
        var storedEntity = dbContext.Tours.FirstOrDefault(i => i.Name == newEntity.Name);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    [Fact]
    public void Create_fails_invalid_data()
    {
        // Arrange
        var userId = -1;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, userId);
        var updatedEntity = new TourDto()
        {
            Description = "Update"
        };

        // Act
        var result = (ObjectResult)controller.Create(updatedEntity).Result;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(400);
    }

    [Fact]
    public void Updates()
    {
        // Arrange
        var userId = -1;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, userId);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var updatedEntity = new TourDto()
        {
            Id = -1,
            UserId = 1,
            Name = "Test",
            Description = "Test",
            Price = 0,
            Duration = 100,
            Distance = 2.1,
            Difficulty = "EASY",
            TransportType = "CAR",
            Status = "DRAFT",
            Tags = new(),
            StatusUpdateTime = DateTime.Now.ToUniversalTime()
        };

        // Act
        var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as TourDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-1);
        result.Name.ShouldBe(updatedEntity.Name);
        result.Description.ShouldBe(updatedEntity.Description);
        result.Price.ShouldBe(updatedEntity.Price);
        result.Difficulty.ShouldBe(updatedEntity.Difficulty);
        result.TransportType.ShouldBe(updatedEntity.TransportType);
        result.Status.ShouldBe(updatedEntity.Status);

        // Assert - Database
        var storedEntity = dbContext.Tours.FirstOrDefault(i => i.Name == "Test");
        storedEntity.ShouldNotBeNull();
        storedEntity.Description.ShouldBe(updatedEntity.Description);
        var oldEntity = dbContext.Tours.FirstOrDefault(i => i.Name == "Test123");
        oldEntity.ShouldBeNull();
    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        // Arrange
        var userId = -1;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, userId);
        var updatedEntity = new TourDto()
        {
            Id = 1,
            UserId = 1,
            Name = "Test",
            Description = "Test",
            Price = 0,
            Duration = 100,
            Distance = 2.1,
            Difficulty = "EASY",
            TransportType = "CAR",
            Status = "DRAFT",
            Tags = new(),
            StatusUpdateTime = DateTime.Now.ToUniversalTime()
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
        var result = (OkResult)controller.Delete(-1);

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedCourse = dbContext.Tours.FirstOrDefault(i => i.Id == -1);
        storedCourse.ShouldBeNull();
    }

    [Fact]
    public void Delete_fails_invalid_id()
    {
        // Arrange
        var userId = -1;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, userId);

        // Act
        var result = (ObjectResult)controller.Delete(-1000);

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    [Fact]
    public void Creates_custom()
    {
        // Arrange
        var userId = -1;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, userId);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var newEntity = new TourDto()
        {
            UserId = 0,
            Name = "Custom tour 1",
            Description = "This is my custom tour",
            Price = 0,
            Duration = 100,
            Distance = 2.1,
            Difficulty = "MEDIUM",
            TransportType = "WALK",
            Status = "CUSTOM",
            Tags = new(),
            StatusUpdateTime = DateTime.Now.ToUniversalTime()
        };

        // Act
        var result = ((ObjectResult)controller.CreateCustomTour(newEntity).Result)?.Value as TourDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Name.ShouldBe(newEntity.Name);

        // Assert - Database
        var storedEntity = dbContext.Tours.FirstOrDefault(i => i.Name == newEntity.Name);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    private static TourManagementController CreateController(IServiceScope scope, int userId)
    {
        return new TourManagementController(scope.ServiceProvider.GetRequiredService<ITourService>())
        {
            ControllerContext = BuildContext(userId.ToString())
        };
    }
}