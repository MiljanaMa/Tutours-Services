using Explorer.API.Controllers.Tourist;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Tourist;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.Tourist;

[Collection("Sequential")]
public class ClubJoinRequestCommandTests : BaseStakeholdersIntegrationTest
{
    public ClubJoinRequestCommandTests(StakeholdersTestFactory factory) : base(factory)
    {
    }

    // [Fact]
    // public void Creates()
    // {
    //     // Arrange
    //     using var scope = Factory.Services.CreateScope();
    //     var controller = CreateController(scope);
    //     var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
    //     var club = new ClubDto
    //     {
    //         Id = -2,
    //         Name = "Klub 2",
    //         Description = "Dobar",
    //         Image = "klub2.png",
    //         UserId = -3,
    //         MemberIds = new List<int> { 1, 2 }
    //     };
    //
    //     // Act
    //     var result = ((ObjectResult)controller.Create(club).Result)?.Value as ClubJoinRequestDto;
    //
    //     // Assert - Response
    //     result.ShouldNotBeNull();
    //     result.ClubId.ShouldBe(club.Id);
    //
    //     // Assert - Database
    //     var storedEntity =
    //         dbContext.ClubJoinRequests.FirstOrDefault(i => i.UserId == result.UserId && i.ClubId == result.ClubId);
    //     storedEntity.ShouldNotBeNull();
    // }

    // [Fact]
    // public void Create_fails_invalid_data()
    // {
    //     // Arrange
    //     using var scope = Factory.Services.CreateScope();
    //     var controller = CreateController(scope);
    //     var club = new ClubDto
    //     {
    //         Id = -1,
    //         Name = "Klub 1",
    //         Description = "Dobar",
    //         Image = "klub1.png",
    //         UserId = -2,
    //         MemberIds = new List<int> { 1, 2 }
    //     };
    //
    //     // Act
    //     var result = (ObjectResult)controller.Create(club).Result;
    //
    //     // Assert
    //     result.StatusCode.ShouldBe(409);
    // }

    [Fact]
    public void Updates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var updatedEntity = new ClubJoinRequestDto
        {
            Id = -1,
            UserId = -21,
            ClubId = -1,
            Status = ClubJoinRequestDto.JoinRequestStatus.Accepted
        };

        // Act
        var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as ClubJoinRequestDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.UserId.ShouldBe(updatedEntity.UserId);
        result.Status.ShouldBe(updatedEntity.Status);

        // Assert - Database
        var storedEntity = (ClubJoinRequest)dbContext.ClubJoinRequests.FirstOrDefault(i =>
            i.UserId == updatedEntity.UserId && i.ClubId == updatedEntity.ClubId);
        storedEntity.ShouldNotBeNull();
        storedEntity.Status.ToString().ShouldBe(updatedEntity.Status.ToString());
    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new ClubJoinRequestDto
        {
            Id = -100,
            UserId = -22,
            ClubId = -1
        };

        // Act
        var result = (ObjectResult)controller.Update(updatedEntity).Result;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }


    private static ClubJoinRequestController CreateController(IServiceScope scope)
    {
        return new ClubJoinRequestController(scope.ServiceProvider.GetRequiredService<IClubJoinRequestService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}