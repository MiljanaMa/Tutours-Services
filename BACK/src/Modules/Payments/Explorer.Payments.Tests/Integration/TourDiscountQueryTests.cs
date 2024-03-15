using Explorer.API.Controllers.Tourist.Marketplace;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.UseCases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Payments.Tests.Integration;

public class TourDiscountQueryTests : BasePaymentsIntegrationTest
{
    public TourDiscountQueryTests(PaymentsTestFactory factory) : base(factory)
    {

    }

    [Fact]
    public void Retrieves_tours_from_other_discounts()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var actionResult = controller.GetToursFromOtherDiscounts(-1);
        var objectResult = actionResult.Result as ObjectResult;

        // Assert
        objectResult.ShouldNotBeNull();
        var value = objectResult.Value as List<int>;
        value.Count.ShouldBe(2);
    }
    private static TourDiscountController CreateController(IServiceScope scope)
    {
        return new TourDiscountController(scope.ServiceProvider.GetRequiredService<ITourDiscountService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}