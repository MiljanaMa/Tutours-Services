using Explorer.API.Controllers.Tourist.Marketplace;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.UseCases;
using Explorer.Payments.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Payments.Tests.Integration;

public class DiscountCommandTests : BasePaymentsIntegrationTest
{
    public DiscountCommandTests(PaymentsTestFactory factory) : base(factory) { }

    [Fact]
    public void Creates()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

        var Discount = new DiscountDto()
        {
            Id = -5,
            StartDate = DateOnly.Parse("1/1/2023"),
            EndDate = DateOnly.Parse("1/1/2024"),
            Percentage = 10,
            UserId = -11
        };

        var result = ((ObjectResult)controller.Create(Discount).Result)?.Value as DiscountDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Percentage.ShouldBe(Discount.Percentage);

        // Assert - Database
        var storedEntity = dbContext.Discounts.FirstOrDefault(i => i.Id == -5);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    [Fact]
    public void Updates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var updatedEntity = new DiscountDto
        {
            Id = -1,
            StartDate = DateOnly.Parse("2/2/2023"),
            EndDate = DateOnly.Parse("3/3/2023"),
            Percentage = 25
        };

        // Act
        var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as DiscountDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-1);
        result.Percentage.ShouldBe(updatedEntity.Percentage);
        result.StartDate.ShouldBe(updatedEntity.StartDate);
        result.EndDate.ShouldBe(updatedEntity.EndDate);
    }
    [Fact]
    public void Deletes()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

        var result = ((OkResult)controller.Delete(id: -1));
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedCourse = dbContext.Discounts.FirstOrDefault(i => i.Id == -1);
        storedCourse.ShouldBeNull();
    }
    private static DiscountController CreateController(IServiceScope scope)
    {
        return new DiscountController(scope.ServiceProvider.GetRequiredService<IDiscountService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}