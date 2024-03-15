using Explorer.API.Controllers.Tourist.TourExecution;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.TourExecution;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.TourExecution;

public class TourIssueCommandTests : BaseStakeholdersIntegrationTest
{
    public TourIssueCommandTests(StakeholdersTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Creates()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var newEntity = new TourIssueDto
        {
            Category = "Test",
            Priority = 3,
            Description = "Test",
            CreationDateTime = DateTime.Now.ToUniversalTime(),
            ResolveDateTime = null,
            IsResolved = false,
            UserId = -21
        };

        //Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TourIssueDto;

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
        var updatedEntity = new TourIssueDto
        {
            Category = "",
            Description = ""
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
        var updatedEntity = new TourIssueDto
        {
            Id = -1,
            Category = "Travel problem",
            Priority = 3,
            Description = "Bus delays",
            CreationDateTime = DateTime.Now.ToUniversalTime(),
            ResolveDateTime = DateTime.MinValue,
            IsResolved = false,
            TourId = 1,
            UserId = -21
        };

        //Act
        var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as TourIssueDto;

        //Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-1);
        result.Category.ShouldBe(updatedEntity.Category);
        result.Priority.ShouldBe(updatedEntity.Priority);
        result.Description.ShouldBe(updatedEntity.Description);
        result.CreationDateTime.ShouldBe(updatedEntity.CreationDateTime);
        result.ResolveDateTime.ShouldBe(updatedEntity.ResolveDateTime);
        result.IsResolved.ShouldBe(updatedEntity.IsResolved);
        //Assert - Database
        var storedEntity = dbContext.TourIssue.FirstOrDefault(i => i.Category == "Travel problem");
        storedEntity.ShouldNotBeNull();
        storedEntity.Priority.ShouldBe(updatedEntity.Priority);
        var oldEntity = dbContext.TourIssue.FirstOrDefault(i => i.Category == "Hygiene");
        oldEntity.ShouldBeNull();
    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new TourIssueDto
        {
            Id = -1000,
            Category = "Required",
            Description = "Required",
            Priority = 4,
            CreationDateTime = DateTime.Now.ToUniversalTime(),
            ResolveDateTime = null,
            IsResolved = false
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
        var storedCourse = dbContext.TourIssue.FirstOrDefault(i => i.Id == -2);
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

    private static TourIssueController CreateController(IServiceScope scope)
    {
        return new TourIssueController(scope.ServiceProvider.GetRequiredService<ITourIssueService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}