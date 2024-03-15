using Explorer.API.Controllers.Author;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourAuthoring;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.TourAuthoring;

public class KeypointCommandTests : BaseToursIntegrationTest
{
    public KeypointCommandTests(ToursTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Creates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var newEntity = new KeypointDto
        {
            Name = "Juliet's balcony",
            TourId = -1,
            Latitude = 33.2,
            Longitude = 12.1,
            Description =
                "When it comes to romance, some seem to prefer fantasy to reality. The house in Verona that has been billed as Juliet’s, is on the whole fluff covered with touristic fairy dust. Shakespeare’s Juliet wasn’t based on a real person, and the house doesn’t have any relation to the story.",
            Position = 3,
            Image = "image.png"
        };

        // Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as KeypointDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Name.ShouldBe(newEntity.Name);

        // Assert - Database
        var storedEntity = dbContext.Keypoints.FirstOrDefault(i => i.Name == newEntity.Name);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    [Fact]
    public void Create_fails_invalid_data()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new KeypointDto
        {
            Description = "Test"
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
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var updatedEntity = new KeypointDto
        {
            Id = -1,
            Name = "Niagara Falls",
            TourId = -1,
            Latitude = 33.1,
            Longitude = 0.0,
            Description =
                "Niagara Falls is a group of three waterfalls at the southern end of Niagara Gorge, spanning the border between the province of Ontario in Canada and the state of New York in the United States. The largest of the three is Horseshoe Falls, which straddles the international border of the two countries.",
            Position = 1,
            Image = "image123.png"
        };

        // Act
        var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as KeypointDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-1);
        result.Name.ShouldBe(updatedEntity.Name);
        result.Latitude.ShouldBe(updatedEntity.Latitude);
        result.Longitude.ShouldBe(updatedEntity.Longitude);
        result.Description.ShouldBe(updatedEntity.Description);

        // Assert - Database
        var storedEntity = dbContext.Keypoints.FirstOrDefault(i => i.Name == "Niagara Falls");
        storedEntity.ShouldNotBeNull();
        storedEntity.Description.ShouldBe(updatedEntity.Description);
        var oldEntity = dbContext.Keypoints.FirstOrDefault(i => i.Name == "Colosseum");
        oldEntity.ShouldBeNull();
    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new KeypointDto
        {
            Id = -1000,
            Name = "Niagara Falls",
            Latitude = 33.1,
            Longitude = 0.0,
            Description =
                "Niagara Falls is a group of three waterfalls at the southern end of Niagara Gorge, spanning the border between the province of Ontario in Canada and the state of New York in the United States. The largest of the three is Horseshoe Falls, which straddles the international border of the two countries."
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
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        // Act
        var result = (OkResult)controller.Delete(-3);

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedCourse = dbContext.Keypoints.FirstOrDefault(i => i.Id == -3);
        storedCourse.ShouldBeNull();
    }

    [Fact]
    public void Delete_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = (ObjectResult)controller.Delete(-1000);

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    private static KeypointController CreateController(IServiceScope scope)
    {
        return new KeypointController(scope.ServiceProvider.GetRequiredService<IKeypointService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}