using Explorer.API.Controllers.Tourist;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Explorer.Payments.Tests.Integration;

[Collection("Sequential")]
public class OrderItemQueryTests : BasePaymentsIntegrationTest
{
    public OrderItemQueryTests(PaymentsTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Retrieves_all()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as PagedResult<OrderItemDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(3);
        result.TotalCount.ShouldBe(3);
    }

    private static OrderItemController CreateController(IServiceScope scope)
    {
        return new OrderItemController(scope.ServiceProvider.GetRequiredService<IOrderItemService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}