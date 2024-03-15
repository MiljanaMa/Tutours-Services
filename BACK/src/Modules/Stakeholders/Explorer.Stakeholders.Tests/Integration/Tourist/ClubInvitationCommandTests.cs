using Explorer.API.Controllers.Tourist;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Tourist;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.Tourist;

[Collection("Sequential")]
public class ClubInvitationCommandTests : BaseStakeholdersIntegrationTest
{
    public ClubInvitationCommandTests(StakeholdersTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Creates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var newEntity = new ClubInvitationDto
        {
            ClubId = 20,
            UserId = 1,
            Status = InvitationStatus.PENDING
        };

        // Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as ClubInvitationDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.ClubId.ShouldBe(newEntity.ClubId);
        result.UserId.ShouldBe(newEntity.UserId);

        // Assert - Database
        var storedEntity =
            dbContext.ClubInvitations.FirstOrDefault(i => i.ClubId == newEntity.ClubId && i.UserId == newEntity.UserId);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    [Fact]
    public void Updates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var updatedEntity = new ClubInvitationDto
        {
            Id = -1,
            ClubId = 3,
            UserId = 3,
            Status = InvitationStatus.DENIED
        };

        // Act
        var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as ClubInvitationDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-1);
        result.ClubId.ShouldBe(updatedEntity.ClubId);
        result.UserId.ShouldBe(updatedEntity.UserId);
        result.Status.ShouldBe(updatedEntity.Status);

        // Assert - Database
        var storedEntity = dbContext.ClubInvitations.FirstOrDefault(i => i.Id == -1);
        storedEntity.ShouldNotBeNull();
        storedEntity.ClubId.ShouldBe(updatedEntity.ClubId);
    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new ClubInvitationDto
        {
            Id = -1000,
            ClubId = 5
        };

        // Act
        var result = (ObjectResult)controller.Update(updatedEntity).Result;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    private static ClubInvitationController CreateController(IServiceScope scope)
    {
        return new ClubInvitationController(scope.ServiceProvider.GetRequiredService<IClubInvitationService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}