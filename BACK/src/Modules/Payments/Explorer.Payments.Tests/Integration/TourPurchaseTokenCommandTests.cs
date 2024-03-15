using Explorer.API.Controllers.Tourist.Marketplace;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Explorer.Payments.Tests.Integration;

public class TourPurchaseTokenCommandTests : BasePaymentsIntegrationTest
{
    public TourPurchaseTokenCommandTests(PaymentsTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Creates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

        List<CouponDto> coupons = new List<CouponDto>();

        // Act
        var result = (ObjectResult)controller.BuyShoppingCart(-1);

        // Assert - Response
        result.ShouldNotBeNull();

        var tokensNumber = dbContext.TourPurchaseTokens.Count();
        tokensNumber.ShouldBe(4);

        // Assert - Database
        var storedEntity = dbContext.TourPurchaseTokens.FirstOrDefault(i => i.TourId == -1);
        storedEntity.ShouldNotBeNull();
    }

    [Fact]
    public void Create_fails_invalid_data_shopping_cart()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        List<CouponDto> coupons = new List<CouponDto>();

        // Act
        var result = (ObjectResult)controller.BuyShoppingCart(-111);

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    [Fact]
    public void Create_fails_invalid_data_order_item()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        List<CouponDto> coupons = new List<CouponDto>();

        // Act
        var result = ((ObjectResult)controller.BuyShoppingCart(-3));

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    [Fact]
    public void Create_fails_invalid_data_token_exists()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        List<CouponDto> coupons = new List<CouponDto>();

        // Act
        var result = (ObjectResult)controller.BuyShoppingCart(-1);

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    private static TourPurchaseTokenController CreateController(IServiceScope scope)
    {
        return new TourPurchaseTokenController(scope.ServiceProvider.GetRequiredService<ITourPurchaseTokenService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}