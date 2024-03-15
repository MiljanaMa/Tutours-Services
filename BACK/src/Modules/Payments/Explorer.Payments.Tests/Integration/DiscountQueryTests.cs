using Explorer.API.Controllers.Tourist.Marketplace;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Payments.Tests.Integration;

public class DiscountQueryTests : BasePaymentsIntegrationTest
{

    public DiscountQueryTests(PaymentsTestFactory factory) : base(factory)
    {

    }
    [Fact]
    public void Retrieves_all()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as PagedResult<DiscountDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(4);
        result.TotalCount.ShouldBe(4);
    }

    private static DiscountController CreateController(IServiceScope scope)
    {
        return new DiscountController(scope.ServiceProvider.GetRequiredService<IDiscountService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}