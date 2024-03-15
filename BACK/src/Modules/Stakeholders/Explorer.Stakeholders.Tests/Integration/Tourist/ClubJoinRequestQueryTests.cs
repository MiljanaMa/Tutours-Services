using Explorer.API.Controllers.Tourist;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Tourist;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.Tourist;

[Collection("Sequential")]
public class ClubJoinRequestQueryTests : BaseStakeholdersIntegrationTest
{
    public ClubJoinRequestQueryTests(StakeholdersTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Retrieves_all()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as PagedResult<ClubJoinRequestDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(4);
        result.TotalCount.ShouldBe(4);
    }

    private static ClubJoinRequestController CreateController(IServiceScope scope)
    {
        return new ClubJoinRequestController(scope.ServiceProvider.GetRequiredService<IClubJoinRequestService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}