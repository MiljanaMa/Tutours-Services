using Explorer.API.Controllers.Tourist.Marketplace;
using Explorer.API.Controllers.Tourist.Shopping;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.MarketPlace;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Tests.Integration.Marketplace
{
    public class BundleTests : BaseToursIntegrationTest
    {
        public BundleTests(ToursTestFactory factory) : base(factory)
        {
        }

        private static BundleController CreateController(IServiceScope scope)
        {
            return new BundleController(scope.ServiceProvider.GetRequiredService<IBundleService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }

        [Fact]
        public void Retrieves_all()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = ((ObjectResult)controller.GetPaged(0, 0).Result)?.Value as PagedResult<BundleDto>;

            // Assert
            result.ShouldNotBeNull();
            //result.Results.Count.ShouldBe(7);
            result.TotalCount.ShouldBe(7);
        }

        [Fact]
        public void Retrieves()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = ((ObjectResult)controller.Get(-1).Result)?.Value as BundleDto;

            // Assert
            result.ShouldNotBeNull();
            //result.Results.Count.ShouldBe(7);
            
        }

    }
}
