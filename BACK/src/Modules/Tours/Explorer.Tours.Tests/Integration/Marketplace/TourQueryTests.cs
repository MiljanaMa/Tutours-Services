using Explorer.API.Controllers.Tourist.Marketplace;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourAuthoring;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.Marketplace;

public class TourQueryTests : BaseToursIntegrationTest
{
    public TourQueryTests(ToursTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Retrieves_all_published()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetPublished(0, 0).Result)?.Value as PagedResult<TourDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(3);
        result.TotalCount.ShouldBe(3);
    }

    [Fact]
    public void Retrieves_all_archived_and_published()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var result =
            ((ObjectResult)controller.GetArchivedAndPublishedPaged(0, 0).Result)?.Value as PagedResult<TourDto>;

        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(3);
        result.TotalCount.ShouldBe(3);
    }

    [Fact]
    public void Retrieves_all_by_author()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var result =
            ((ObjectResult)controller.GetByAuthor(1, 20, 1).Result)?.Value as PagedResult<TourDto>;

        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(3);
        result.TotalCount.ShouldBe(3);
    }

    private static TourController CreateController(IServiceScope scope)
    {
        return new TourController(scope.ServiceProvider.GetRequiredService<ITourService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}