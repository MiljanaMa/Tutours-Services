using Explorer.API.Controllers.Tourist;
using Explorer.API.Controllers.Tourist.Marketplace;
using Explorer.Blog.Infrastructure.Database;
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

namespace Explorer.Payments.Tests.Integration
{
    [Collection("Sequential")]
    public class CouponCommandTests : BasePaymentsIntegrationTest
    {
        public CouponCommandTests(PaymentsTestFactory factory) : base(factory)
        {
        }

        [Fact]
        public void Creates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
            var newEntity = new API.Dtos.CouponDto(-3, "CCCCCCCC", 15, -1, -21, -11, new DateOnly(2024, 11, 11));

            // Act
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as CouponDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(-3);

            // Assert - Database
            var storedEntity = dbContext.Coupons.FirstOrDefault(i => i.Id == newEntity.Id);
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
            var updatedEntity = new API.Dtos.CouponDto(-1, "AAAAAAAA", 20, -1, -21, -11, new DateOnly(2024, 11, 11));

            // Act
            var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as CouponDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(-1);
            result.TourId.ShouldBe(updatedEntity.TourId);
            result.TouristId.ShouldBe(updatedEntity.TouristId);
            result.AuthorId.ShouldBe(updatedEntity.AuthorId);
            result.Code.ShouldBe(updatedEntity.Code);
            result.ExpiryDate.ShouldBe(updatedEntity.ExpiryDate);

            // Assert - Database
            var storedEntity = dbContext.Coupons.FirstOrDefault(i => i.Id == -1);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.Id);
        }

        [Fact]
        public void Deletes()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

            //Act
            var result = (OkResult)controller.Delete(-1);

            //Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            //Assert - Database
            var deletedEntity = dbContext.Coupons.FirstOrDefault(i => i.Id == -1);
            deletedEntity.ShouldBeNull();
        }

        private static CouponController CreateController(IServiceScope scope)
        {
            return new CouponController(scope.ServiceProvider.GetRequiredService<ICouponService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}