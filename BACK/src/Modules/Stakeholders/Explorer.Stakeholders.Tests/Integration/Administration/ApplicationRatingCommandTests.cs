using Explorer.API.Controllers.Tourist;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.Administration;

[Collection("Sequential")]
public class ApplicationRatingCommandTests : BaseStakeholdersIntegrationTest
{
    public ApplicationRatingCommandTests(StakeholdersTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Creates()
    {
        // Arrange
        var userId = -1;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, userId);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var newEntity = new ApplicationRatingDto
        {
            Id = 12,
            Rating = 4,
            Comment = "Very good!",
            UserId = userId,
            LastModified = DateTime.UtcNow
        };

        // Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as ApplicationRatingDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Id.ShouldBe(newEntity.Id);

        // Assert - Database
        var storedEntity = dbContext.ApplicationRatings.FirstOrDefault(i => i.Id == newEntity.Id);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    [Fact]
    public void Create_fails_rating_exists()
    {
        // Arrange
        var userId = -2;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, userId);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var newEntity = new ApplicationRatingDto
        {
            Id = 13,
            Rating = 4,
            Comment = "Very good!",
            UserId = userId,
            LastModified = DateTime.UtcNow
        };

        // Act
        var result = (ObjectResult)controller.Create(newEntity).Result;

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(409);
    }

    [Fact]
    public void Updates()
    {
        // Arrange
        var userId = -2;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, userId);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var updatedEntity = new ApplicationRatingDto
        {
            Id = -22,
            Rating = 5,
            Comment = "Great!",
            UserId = userId,
            LastModified = DateTime.UtcNow
        };

        // Act
        var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as ApplicationRatingDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(updatedEntity.Id);
        result.Rating.ShouldBe(updatedEntity.Rating);
        result.Comment.ShouldBe(updatedEntity.Comment);
        result.UserId.ShouldBe(updatedEntity.UserId);
        result.LastModified.ShouldBe(updatedEntity.LastModified);

        // Assert - Database
        var storedEntity = dbContext.ApplicationRatings.FirstOrDefault(i => i.Id == updatedEntity.Id);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
        storedEntity.Rating.ShouldBe(result.Rating);
        storedEntity.Comment.ShouldBe(result.Comment);
        storedEntity.UserId.ShouldBe(result.UserId);
        storedEntity.LastModified.ShouldBe(result.LastModified);
    }

    [Fact]
    public void Update_fails_rating_missing()
    {
        // Arrange
        var userId = -7;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, userId);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var updatedEntity = new ApplicationRatingDto
        {
            Id = 77,
            Rating = 5,
            Comment = "Great!",
            UserId = userId,
            LastModified = DateTime.UtcNow
        };

        // Act
        var result = (ObjectResult)controller.Update(updatedEntity).Result;

        // Assert - Response
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
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var entity = new ApplicationRatingDto
        {
            Id = -21,
            Rating = 5,
            Comment = "I like it!",
            UserId = userId,
            LastModified = DateTime.UtcNow
        };

        // Act
        var result = (OkResult)controller.Delete();

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedCourse = dbContext.ApplicationRatings.FirstOrDefault(i => i.Id == entity.Id);
        storedCourse.ShouldBeNull();
    }

    private static ApplicationRatingController CreateController(IServiceScope scope, int userId)
    {
        return new ApplicationRatingController(scope.ServiceProvider.GetRequiredService<IApplicationRatingService>())
        {
            ControllerContext = BuildContext(userId.ToString())
        };
    }
}