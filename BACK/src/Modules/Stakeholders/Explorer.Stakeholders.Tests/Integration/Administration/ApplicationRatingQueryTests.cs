using Explorer.API.Controllers.Administrator;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.Administration;

[Collection("Sequential")]
public class ApplicationRatingQueryTests : BaseStakeholdersIntegrationTest
{
    public ApplicationRatingQueryTests(StakeholdersTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Retrieves_all()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as PagedResult<ApplicationRatingDto>;

        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(3);
        result.TotalCount.ShouldBe(3);
    }

    private static ApplicationRatingController CreateController(IServiceScope scope)
    {
        return new ApplicationRatingController(scope.ServiceProvider.GetRequiredService<IApplicationRatingService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}