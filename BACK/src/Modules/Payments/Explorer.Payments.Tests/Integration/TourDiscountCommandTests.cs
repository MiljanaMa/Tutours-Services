using Explorer.API.Controllers.Tourist.Marketplace;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.UseCases;
using Explorer.Payments.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Payments.Tests.Integration;

public class TourDiscountCommandTests : BasePaymentsIntegrationTest
{

    public TourDiscountCommandTests(PaymentsTestFactory factory) : base(factory) { }

    [Fact]
    public void Creates()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

        var tourDiscount = new TourDiscountDto
        {
            DiscountId = -3,
            TourId = -4
        };

        var result = ((ObjectResult)controller.Create(tourDiscount).Result)?.Value as TourDiscountDto;

        // Assert - Response
        result.ShouldNotBeNull();

        // Assert - Database
        var storedEntity = dbContext.TourDiscounts.FirstOrDefault(i => i.TourId == tourDiscount.TourId);
        storedEntity.ShouldNotBeNull();
    }

    [Fact]
    public void Deletes()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

        var result = ((OkResult)controller.Delete(-1));
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedDiscount = dbContext.TourDiscounts.FirstOrDefault(td => td.TourId == -1);
        storedDiscount.ShouldBeNull();
    }


    private static TourDiscountController CreateController(IServiceScope scope)
    {
        return new TourDiscountController(scope.ServiceProvider.GetRequiredService<ITourDiscountService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}