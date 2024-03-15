using Explorer.API.Controllers.Tourist;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Tourist;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.Tourist;

[Collection("Sequential")]
public class ClubCommandTests : BaseStakeholdersIntegrationTest
{
    public ClubCommandTests(StakeholdersTestFactory factory) : base(factory)
    {
    }

    // [Fact]
    // public void Creates()
    // {
    //     // Arrange
    //     using var scope = Factory.Services.CreateScope();
    //     var controller = CreateController(scope);
    //     var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
    //     var newEntity = new ClubDto
    //     {
    //         Name = "Klub55",
    //         Description = "Mid",
    //         UserId = 10,
    //         MemberIds = new List<int>()
    //     };
    //
    //     // Act
    //     var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as ClubDto;
    //
    //     // Assert - Response
    //     result.ShouldNotBeNull();
    //     result.Id.ShouldNotBe(0);
    //     result.Name.ShouldBe(newEntity.Name);
    //
    //     // Assert - Database
    //     var storedEntity = dbContext.Clubs.FirstOrDefault(i => i.Name == newEntity.Name);
    //     storedEntity.ShouldNotBeNull();
    //     storedEntity.Id.ShouldBe(result.Id);
    // }
    /*
    [Fact]
    public void Create_fails_invalid_data()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new ClubDto
        {
            Description = "W klub"
        };

        // Act
        var result = (ObjectResult)controller.Create(updatedEntity).Result;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(400);
    }
    */
    // [Fact]
    // public void Updates()
    // {
    //     // Arrange
    //     using var scope = Factory.Services.CreateScope();
    //     var controller = CreateController(scope);
    //     var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
    //     var updatedEntity = new ClubDto
    //     {
    //         Id = -1,
    //         Name = "Klub 384",
    //         Description = "Kul",
    //         UserId = -2
    //     };
    //
    //     // Act
    //     var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as ClubDto;
    //
    //     // Assert - Response
    //     result.ShouldNotBeNull();
    //     result.Id.ShouldBe(-1);
    //     result.Name.ShouldBe(updatedEntity.Name);
    //     result.Description.ShouldBe(updatedEntity.Description);
    //     result.UserId.ShouldBe(updatedEntity.UserId);
    //
    //     // Assert - Database
    //     var storedEntity = dbContext.Clubs.FirstOrDefault(i => i.Name == "Klub 384");
    //     storedEntity.ShouldNotBeNull();
    //     storedEntity.Description.ShouldBe(updatedEntity.Description);
    //     var oldEntity = dbContext.Clubs.FirstOrDefault(i => i.Name == "Klub 1");
    //     oldEntity.ShouldBeNull();
    // }

    [Fact]
    public void Update_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new ClubDto
        {
            Id = -1000,
            Name = "L klub"
        };

        // Act
        var result = (ObjectResult)controller.Update(updatedEntity).Result;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }


    private static ClubController CreateController(IServiceScope scope)
    {
        return new ClubController(scope.ServiceProvider.GetRequiredService<IClubService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}