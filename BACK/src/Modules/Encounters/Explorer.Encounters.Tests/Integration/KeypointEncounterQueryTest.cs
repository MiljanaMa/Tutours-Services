using Explorer.API.Controllers;
using Explorer.API.Controllers.Author;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Encounters.Tests.Integration
{
    public class KeypointEncounterQueryTest : BaseEncountersIntegrationTest
    {
        public KeypointEncounterQueryTest(EncountersTestFactory factory) : base(factory) { }

        [Fact]
        public void Retrieves_all()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = ((ObjectResult)controller.GetPagedByKeypoint(1, 1, -1).Result)?.Value as PagedResult<KeypointEncounterDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(1);
            result.TotalCount.ShouldBe(1);
        }

        private static KeypointEncounterController CreateController(IServiceScope scope)
        {
            return new KeypointEncounterController(scope.ServiceProvider.GetRequiredService<IKeypointEncounterService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
