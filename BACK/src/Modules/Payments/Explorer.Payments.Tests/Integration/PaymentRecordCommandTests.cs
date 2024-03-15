using Explorer.API.Controllers.Tourist;
using Explorer.API.Controllers.Tourist.Marketplace;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Tests.Integration;
[Collection("Sequential")]

public class PaymentRecordCommandTests : BasePaymentsIntegrationTest
{
    public PaymentRecordCommandTests(PaymentsTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Creates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var newEntity = new PaymentRecordDto
        {
            Id = 3,
            UserId = 1,
            TourId = 3,
            TourPrice = 1500,
            PaymentTime = DateTimeOffset.Now.ToUniversalTime()
        };

        // Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as PaymentRecordDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(-1);

        // Assert - Database
        var storedEntity = dbContext.PaymentRecords.FirstOrDefault(i => i.UserId == newEntity.UserId && i.TourId == newEntity.TourId && i.TourPrice == newEntity.TourPrice);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }


    private static PaymentRecordController CreateController(IServiceScope scope)
    {
        return new PaymentRecordController(scope.ServiceProvider.GetRequiredService<IPaymentRecordService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}
