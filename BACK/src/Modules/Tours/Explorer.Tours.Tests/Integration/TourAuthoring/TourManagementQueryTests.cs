using Explorer.API.Controllers.Author;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourAuthoring;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.TourAuthoring;

public class TourManagementQueryTests : BaseToursIntegrationTest
{
    public TourManagementQueryTests(ToursTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Retrieves_all()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as PagedResult<TourDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(3);
        result.TotalCount.ShouldBe(3);
    }

    [Fact]
    public void Retrieves_one()
    {
        //Arrange
        var id = -1;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetById(id).Result)?.Value as TourDto;

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-1);
    }

    private static TourManagementController CreateController(IServiceScope scope)
    {
        return new TourManagementController(scope.ServiceProvider.GetRequiredService<ITourService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}