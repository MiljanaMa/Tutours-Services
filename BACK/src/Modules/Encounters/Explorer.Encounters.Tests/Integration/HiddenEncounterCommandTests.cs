using Explorer.API.Controllers;
using Explorer.API.Controllers.Tourist.Encounters;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using Shouldly;

namespace Explorer.Encounters.Tests.Integration
{
    [Collection("Sequential")]
    public class HiddenEncounterCommandTests: BaseEncountersIntegrationTest
    {
        public HiddenEncounterCommandTests(EncountersTestFactory factory) : base(factory) { }

        [Fact]
        public void Complete()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = ((ObjectResult)controller.CheckNearbyEncounters().Result)?.Value as PagedResult<EncounterCompletionDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(1);
            result.TotalCount.ShouldBe(1);

            result.Results[0].Status.ShouldBe("FINISHED");
        }

        private static EncounterCompletionController CreateController(IServiceScope scope)
        {
            return new EncounterCompletionController(scope.ServiceProvider.GetRequiredService<IEncounterCompletionService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
