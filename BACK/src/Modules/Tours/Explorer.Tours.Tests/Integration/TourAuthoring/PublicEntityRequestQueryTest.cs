using Explorer.API.Controllers.Author;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourAuthoring;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.TourAuthoring;

[Collection("Sequential")]
public class PublicEntityRequestQueryTest : BaseToursIntegrationTest
{
    public PublicEntityRequestQueryTest(ToursTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Retrieves_all()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as PagedResult<PublicEntityRequestDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(4);
        result.TotalCount.ShouldBe(4);
    }

    private static PublicEntityRequestController CreateController(IServiceScope scope)
    {
        return new PublicEntityRequestController(
            scope.ServiceProvider.GetRequiredService<IPublicEntityRequestService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}