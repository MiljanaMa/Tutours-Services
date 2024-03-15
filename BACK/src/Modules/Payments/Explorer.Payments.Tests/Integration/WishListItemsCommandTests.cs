using Explorer.API.Controllers.Tourist.Marketplace;
using Explorer.Payments.API.Public;
using Explorer.Payments.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Explorer.Payments.API.Dtos;
using Explorer.Tours.Core.Domain.Enum;

namespace Explorer.Payments.Tests.Integration;

[Collection("Sequential")]
public class WishListItemsCommandTests : BasePaymentsIntegrationTest
{
    public WishListItemsCommandTests(PaymentsTestFactory factory): base(factory)
    {

    }

    [Fact]
    public void Creates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var newEntity = new WishListItemDto
        {
            Id = 7,
            TourId = 4,
            TourName = "Tura1",
            TourPrice = 12,
            TourDescription = "skroz dobra tura",
            UserId = 1,
            TourDuration = 12,
            TravelDistance = 12,
            TourDifficulty = "0",
            TourType = "0"
        };

        // Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as WishListItemDto;

        // Assert - Response
        result.ShouldNotBeNull();

        // Assert - Database
        var storedEntity = dbContext.WishListItems.FirstOrDefault(i => i.UserId == newEntity.UserId);
        storedEntity.ShouldNotBeNull();
        //var wishList = dbContext.WishLists.FirstOrDefault(i => i.UserId == newEntity.UserId);
        //wishList.ShouldNotBeNull();
    }

    [Fact]
    public void Updates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var updatedEntity = new WishListItemDto
        {
            Id = 111,
            TourId = 1,
            UserId = 1,
            TourPrice = 150,
            TourName = "Tura1",
            TourType = "0",
            TourDifficulty= "0",
            TourDescription = "qq",
            TourDuration= 12,
            TravelDistance = 12
        };

        // Act
        var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as WishListItemDto;

        // Assert - Response
        
         // result.ShouldNotBeNull();
        /*result.Id.ShouldBe(111);
        result.TourName.ShouldBe(updatedEntity.TourName);
        result.UserId.ShouldBe(updatedEntity.UserId);
        result.TourId.ShouldBe(updatedEntity.TourId);
        result.TourPrice.ShouldBe(updatedEntity.TourPrice);*/
        

        // Assert - Database
        var storedEntity = dbContext.WishListItems.FirstOrDefault(i => i.UserId == 1);
        storedEntity.ShouldNotBeNull();
        storedEntity.TourName.ShouldBe(updatedEntity.TourName);
        storedEntity.TourId.ShouldBe(updatedEntity.TourId);
        storedEntity.UserId.ShouldBe(updatedEntity.UserId);
        storedEntity.TourDescription.ShouldBe(updatedEntity.TourDescription);
    }



    private static WishListItemController CreateController(IServiceScope scope)
    {
        return new WishListItemController(scope.ServiceProvider.GetRequiredService<IWishListItemService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}
