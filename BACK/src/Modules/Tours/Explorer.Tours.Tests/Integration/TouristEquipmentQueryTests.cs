using Explorer.API.Controllers.Tourist;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration;

public class TouristEquipmentQueryTests : BaseToursIntegrationTest
{
    public TouristEquipmentQueryTests(ToursTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Retrieves_all()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result =
            ((ObjectResult)controller.GetAllForSelected(-23).Result)?.Value as IEnumerable<EquipmentForSelectionDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(3);

        result.ElementAt(0).IsSelected.ShouldBeTrue();
        result.ElementAt(1).IsSelected.ShouldBeTrue();
        result.ElementAt(2).IsSelected.ShouldBeFalse();
    }

    private static TouristEquipmentController CreateController(IServiceScope scope)
    {
        return new TouristEquipmentController(scope.ServiceProvider.GetRequiredService<ITouristEquipmentService>(),
            scope.ServiceProvider.GetRequiredService<IEquipmentService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}