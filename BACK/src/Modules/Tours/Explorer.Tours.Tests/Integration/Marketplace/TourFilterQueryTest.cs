using Explorer.API.Controllers.Tourist.Marketplace;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.MarketPlace;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.Marketplace;

public class TourFilterQueryTest : BaseToursIntegrationTest
{
    public TourFilterQueryTest(ToursTestFactory factory) : base(factory)
    {
    }


    [Fact]
    public void Retrieves()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var criteria = new FilterCriteriaDto();
        criteria.CurrentLatitude = 20;
        criteria.CurrentLongitude = -10;
        criteria.FilterRadius = 1500;
        var result = ((ObjectResult)controller.GetFilteredTours(1, 5, criteria).Result)?.Value as PagedResult<TourDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(2);
        result.TotalCount.ShouldBe(2);
    }

    [Fact]
    public void Retrieves_fail()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var criteria = new FilterCriteriaDto();
        criteria.CurrentLatitude = 180;
        criteria.CurrentLongitude = -10;
        criteria.FilterRadius = 10;
        var result = ((ObjectResult)controller.GetFilteredTours(1, 5, criteria).Result)?.Value as PagedResult<TourDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(0);
        result.TotalCount.ShouldBe(0);
    }

    private static TourFilteringController CreateController(IServiceScope scope)
    {
        return new TourFilteringController(scope.ServiceProvider.GetRequiredService<ITourFilteringService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}