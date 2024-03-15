using Explorer.API.Controllers.Tourist.TourExecution;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.TourExecution;
using Explorer.Stakeholders.Tests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.TourExecution;

public class TourIssueCommentQueryTests : BaseStakeholdersIntegrationTest
{
    public TourIssueCommentQueryTests(StakeholdersTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Retrieves_all()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        //Act
        var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as PagedResult<TourIssueCommentDto>;

        //Assert
        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(3);
        result.TotalCount.ShouldBe(3);
    }

    private static TourIssueCommentController CreateController(IServiceScope scope)
    {
        return new TourIssueCommentController(scope.ServiceProvider.GetRequiredService<ITourIssueCommentService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}