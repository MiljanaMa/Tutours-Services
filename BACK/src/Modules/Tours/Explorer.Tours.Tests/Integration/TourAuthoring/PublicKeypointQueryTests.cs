using Explorer.API.Controllers.Author;
using Explorer.API.Controllers.Tourist;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.MarketPlace;
using Explorer.Tours.API.Public.TourAuthoring;
using Explorer.Tours.Core.Domain.Enum;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Security.Claims;

namespace Explorer.Tours.Tests.Integration.TourAuthoring
{
    [Collection("Sequential")]
    public class PublicKeypointQueryTests : BaseToursIntegrationTest
    {
        public PublicKeypointQueryTests(ToursTestFactory factory) : base(factory) { }

        [Fact]
        public void Retrieves_all()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = ((ObjectResult)controller.GetPaged(0, 0).Result)?.Value as PagedResult<PublicKeypointDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(2);
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
