using Explorer.API.Controllers;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.ClubFights;

[Collection("Sequential")]
public class ClubChallengeRequestCommandTests : BaseStakeholdersIntegrationTest
{
    public ClubChallengeRequestCommandTests(StakeholdersTestFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public void Creates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var newEntity = new ClubChallengeRequestDto()
        {
            Id = -12,
            ChallengerId = -1,
            ChallengedId = -2
        };
    
        // Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as ClubChallengeRequestDto;
    
        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Status.ShouldBe("PENDING");
    }
    
    [Fact]
    public void AcceptsChallenge()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new ClubChallengeRequestDto()
        {
            Id = -1,
            ChallengerId = -1,
            ChallengedId = -2
        };
    
        // Act
        var result = ((ObjectResult)controller.AcceptChallenge(updatedEntity).Result)?.Value as ClubChallengeRequestDto;
    
        // Assert - Response
        result.ShouldNotBeNull();
        result.Status.ShouldBe("ACCEPTED");
    }
    
    private static ClubChallengeRequestController CreateController(IServiceScope scope)
    {
        return new ClubChallengeRequestController(scope.ServiceProvider.GetRequiredService<IClubChallengeRequestService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}