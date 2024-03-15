using Explorer.API.Controllers.Tourist;
using Explorer.API.Controllers.Tourist.Marketplace;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;


namespace Explorer.Payments.Tests.Integration;

[Collection("Sequential")]
public class WishListItemsQueryTests : BasePaymentsIntegrationTest
{
    public WishListItemsQueryTests(PaymentsTestFactory factory): base(factory)
    {

    }

    [Fact]
    public void Retrieves_all()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as PagedResult<WishListItemDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(2);
        result.TotalCount.ShouldBe(2);
    }

    private static WishListItemController CreateController(IServiceScope scope)
    {
        return new WishListItemController(scope.ServiceProvider.GetRequiredService<IWishListItemService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}
