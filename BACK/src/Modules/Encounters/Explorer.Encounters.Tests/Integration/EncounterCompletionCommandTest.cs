using Explorer.API.Controllers.Tourist.Encounters;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Tests.Integration
{
    public class EncounterCompletionCommandTest : BaseEncountersIntegrationTest
    {
        public EncounterCompletionCommandTest(EncountersTestFactory factory) : base(factory) { }
        [Fact]
        public void Start()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
            var encounter = new EncounterDto
            {
                Id = -3,
                UserId = 1,
                Name = "Encounter2",
                Description = "Description3",
                Latitude = 23.34,
                Longitude = 32.34,
                Xp = 2,
                Status = "ACTIVE",
                Type = "MISC",
                Range = 1070
            };

            // Act
            var result = ((ObjectResult)controller.StartEncounter(encounter).Result)?.Value as EncounterCompletionDto;

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe("STARTED");

            // Assert - Database
            var storedEntity = dbContext.EncounterCompletions.FirstOrDefault(i => i.UserId == -1  && i.EncounterId == -3);
            storedEntity.ShouldNotBeNull();

        }

        [Fact]
        public void Finish()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
            var encounter = new EncounterDto
            {
                Id = -1,
                UserId = 1,
                Name = "Encounter2",
                Description = "Description3",
                Latitude = 23.34,
                Longitude = 32.34,
                Xp = 2,
                Status = "ACTIVE",
                Type = "MISC",
                Range = 1070
            };

            // Act
            var result = ((ObjectResult)controller.FinishEncounter(encounter).Result)?.Value as EncounterCompletionDto;

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe("COMPLETED");

            // Assert - Database
            var storedEntity = dbContext.EncounterCompletions.FirstOrDefault(i => i.UserId == -1 && i.EncounterId == -1);
            storedEntity.ShouldNotBeNull();

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
