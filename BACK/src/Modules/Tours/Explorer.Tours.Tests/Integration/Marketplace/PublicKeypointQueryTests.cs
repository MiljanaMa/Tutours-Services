using Explorer.API.Controllers.Tourist.Marketplace;
using Explorer.API.Controllers.Tourist;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.MarketPlace;
using Explorer.Tours.Core.Domain.Enum;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Security.Claims;
using Explorer.Tours.API.Public.TourAuthoring;
using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Tours.Tests.Integration.Marketplace
{
    [Collection("Sequential")]
    public class PublicKeypointQueryTests : BaseToursIntegrationTest
    {
        public PublicKeypointQueryTests(ToursTestFactory factory) : base(factory) { }

        [Fact]
        public void Retrieves_in_range_none()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var filterCriteria = new FilterCriteriaDto
            {
                CurrentLatitude = 0,
                CurrentLongitude = 0,
                FilterRadius = 2
            };

            // Act
            var result = ((ObjectResult)controller.GetPagedInRange(0, 0, filterCriteria).Result)?.Value as PagedResult<PublicKeypointDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(0);
        }

        [Fact]
        public void Retrieves_in_range_one()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var filterCriteria = new FilterCriteriaDto
            {
                CurrentLatitude = 9.99,
                CurrentLongitude = 9.99,
                FilterRadius = 10
            };

            // Act
            var result = ((ObjectResult)controller.GetPagedInRange(0, 0, filterCriteria).Result)?.Value as PagedResult<PublicKeypointDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(1);
        }

        private static PublicKeypointController CreateController(IServiceScope scope)
        {
            return new PublicKeypointController(scope.ServiceProvider.GetRequiredService<IPublicKeypointService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
