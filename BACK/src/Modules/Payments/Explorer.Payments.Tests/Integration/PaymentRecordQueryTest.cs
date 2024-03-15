using Explorer.API.Controllers.Tourist;
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
using Explorer.API.Controllers.Tourist.Marketplace;

namespace Explorer.Payments.Tests.Integration;
[Collection("Sequential")]

public class PaymentRecordQueryTest : BasePaymentsIntegrationTest
{
    public PaymentRecordQueryTest(PaymentsTestFactory factory) : base(factory) { }

    [Fact]
    public void Retrieves_all()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as PagedResult<PaymentRecordDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(2);
        result.TotalCount.ShouldBe(2);
    }

    private static PaymentRecordController CreateController(IServiceScope scope)
    {
        return new PaymentRecordController(scope.ServiceProvider.GetRequiredService<IPaymentRecordService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}
