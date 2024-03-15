using Explorer.API.Controllers.Author;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Infrastructure.Database;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Encounters.Tests.Integration
{
    [Collection("Sequential")]
    public class KeypointEncounterCommandTest: BaseEncountersIntegrationTest
    {
        public KeypointEncounterCommandTest(EncountersTestFactory factory) : base(factory) { }

        [Fact]
        public void Creates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
            var newEncounter = new EncounterDto
            {
                UserId = -11,
                Name = "Encounter123",
                Description = "Description123",
                Latitude = 54.34,
                Longitude = 34.56,
                Xp = 4,
                Status = "DRAFT",
                Type = "SOCIAL",
                Range = 100,
                Image = null,
                PeopleCount = 12


            };
            var newEntity = new KeypointEncounterDto
            {
                Encounter = newEncounter,
                EncounterId = 0,
                IsRequired = false,
                KeypointId = -3,
            };

            // Act
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as KeypointEncounterDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.Encounter.Name.ShouldBe(newEntity.Encounter.Name);

            // Assert - Database
            var storedEntity = dbContext.KeypointEncounters.FirstOrDefault(i => i.KeyPointId == newEntity.KeypointId);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.Id);
        }

        [Fact]
        public void Updates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
            var encounter = new EncounterDto
            {
                Id = -1,
                UserId = -11,
                Name = "Encounter123",
                Description = "Description123",
                Latitude = 54.34,
                Longitude = 34.56,
                Xp = 4,
                Status = "DRAFT",
                Type = "SOCIAL",
                Range = 100,
                Image = null,
                PeopleCount = 12


            };
            var updatedEntity = new KeypointEncounterDto
            {
                Id = -1,
                Encounter = encounter,
                EncounterId = -1,
                IsRequired = true,
                KeypointId = -1,
            };

            // Act
            var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as KeypointEncounterDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(-1);
            result.IsRequired.ShouldBe(updatedEntity.IsRequired);
            result.KeypointId.ShouldBe(updatedEntity.KeypointId);

            // Assert - Database
            var storedEntity = dbContext.KeypointEncounters.FirstOrDefault(i => i.EncounterId == -1);
            storedEntity.ShouldNotBeNull();
            storedEntity.IsRequired.ShouldBe(updatedEntity.IsRequired);
            var oldEntity = dbContext.Encounters.FirstOrDefault(i => i.Name == "Encounter1");
            oldEntity.ShouldBeNull();
        }



        [Fact]
        public void Update_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var encounter = new EncounterDto
            {
                UserId = -11,
                Name = "Encounter123",
                Description = "Description123",
                Latitude = 54.34,
                Longitude = 34.56,
                Xp = 4,
                Status = "DRAFT",
                Type = "SOCIAL",
                Range = 100,
                Image = null,
                PeopleCount = 12


            };
            var updatedEntity = new KeypointEncounterDto
            {
                Id = -100,
                Encounter = encounter,
                EncounterId = 0,
                IsRequired = false,
                KeypointId = -3,
            };

            // Act
            var result = (ObjectResult)controller.Update(updatedEntity).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }

        [Fact]
        public void UpdateLocation()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
            var location = new LocationDto
            {
                Latitude = 33.34,
                Longitude = 33.56,
            };

            // Act
            var result = (OkResult)controller.UpdateEncounterLocation(location, -1).Result;
            result.StatusCode.ShouldBe(200);

        }


        [Fact]
        public void Deletes()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();

            // Act
            var result = (OkResult)controller.Delete(-2);

            // Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            // Assert - Database
            var storedCourse = dbContext.Encounters.FirstOrDefault(i => i.Id == -2);
            storedCourse.ShouldBeNull();
        }

        [Fact]
        public void Delete_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = (ObjectResult)controller.Delete(-1000);

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
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
