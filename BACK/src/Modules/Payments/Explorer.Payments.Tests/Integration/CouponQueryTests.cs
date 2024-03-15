using Explorer.API.Controllers.Tourist;
using Explorer.API.Controllers.Tourist.Marketplace;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
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
    public class CouponQueryTests : BasePaymentsIntegrationTest
    {
        public CouponQueryTests(PaymentsTestFactory factory) : base(factory) { }

        [Fact]
        public void Retrieves_all_for_tour_and_tourist()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = ((ObjectResult)controller.GetCouponsForTourAndTourist(1, 10, -1, -21).Result)?.Value as PagedResult<CouponDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(2);
            result.TotalCount.ShouldBe(2);
        }

        [Fact]
        public void Retrieves_all_for_author()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = ((ObjectResult)controller.GetCouponsForAuthor(1, 10, -11).Result)?.Value as PagedResult<CouponDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(2);
            result.TotalCount.ShouldBe(2);
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
