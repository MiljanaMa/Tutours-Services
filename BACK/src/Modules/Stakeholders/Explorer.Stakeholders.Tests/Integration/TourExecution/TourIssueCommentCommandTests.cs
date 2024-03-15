using Explorer.API.Controllers.Tourist.TourExecution;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.TourExecution;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.TourExecution;

public class TourIssueCommentCommandTests : BaseStakeholdersIntegrationTest
{
    public TourIssueCommentCommandTests(StakeholdersTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Creates()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var newEntity = new TourIssueCommentDto
        {
            Id = -4,
            TourIssueId = -1,
            UserId = -23,
            Comment = "Comment",
            CreationDateTime = DateTime.UtcNow
        };

        //Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TourIssueCommentDto;

        //Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(result.Id);
    }

    [Fact]
    public void Create_fails_invalid_data()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new TourIssueCommentDto
        {
            Id = -1000,
            TourIssueId = -1,
            UserId = -23,
            Comment = "",
            CreationDateTime = DateTime.UtcNow
        };

        //Act
        var result = (ObjectResult)controller.Create(updatedEntity).Result;

        //Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(400);
    }

    [Fact]
    public void Updates()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var updatedEntity = new TourIssueCommentDto
        {
            Id = -3,
            TourIssueId = -1,
            UserId = -23,
            Comment = "Updated",
            CreationDateTime = DateTime.UtcNow
        };

        //Act
        var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as TourIssueCommentDto;

        //Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-3);
        result.TourIssueId.ShouldBe(-1);
        result.UserId.ShouldBe(-23);
        result.Comment.ShouldBe("Updated");
        //Assert - Database
        var storedEntity = dbContext.TourIssueComments.FirstOrDefault(i => i.Comment == "Updated");
        storedEntity.ShouldNotBeNull();
        var oldEntity = dbContext.TourIssueComments.FirstOrDefault(i => i.Comment == "Comment3");
        oldEntity.ShouldBeNull();
    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new TourIssueCommentDto
        {
            Id = -1000,
            TourIssueId = -1,
            UserId = -23,
            Comment = "Updated",
            CreationDateTime = DateTime.Now
        };

        //Act 
        var result = (ObjectResult)controller.Update(updatedEntity).Result;

        //Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    [Fact]
    public void Deletes()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        //Act
        var result = (OkResult)controller.Delete(-2);

        //Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        //Assert - Database
        var storedCourse = dbContext.TourIssueComments.FirstOrDefault(i => i.Id == -2);
        storedCourse.ShouldBeNull();
    }

    [Fact]
    public void Delete_fails_invalid_id()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        //Act
        var result = (ObjectResult)controller.Delete(-1000);

        //Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    private static TourIssueCommentController CreateController(IServiceScope scope)
    {
        return new TourIssueCommentController(scope.ServiceProvider.GetRequiredService<ITourIssueCommentService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}