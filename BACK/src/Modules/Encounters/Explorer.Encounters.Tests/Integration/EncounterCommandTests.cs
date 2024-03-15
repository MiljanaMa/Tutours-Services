using Explorer.API.Controllers;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Encounters.Tests.Integration
{
    [Collection("Sequential")]
    public class EncounterCommandTests : BaseEncountersIntegrationTest
    {
        public EncounterCommandTests(EncountersTestFactory factory) : base(factory) { }

        [Fact]
        public void Creates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
            var newEntity = new EncounterDto
            {
                UserId = -1,
                Name = "Encounter4",
                Description = "Description4",
                Latitude = 54.34,
                Longitude = 34.56,
                Xp = 4,
                Status = "DRAFT",
                Type = "SOCIAL",
                ApprovalStatus = "PENDING"
            };

            // Act
            //for some reason it won't get user by id
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as EncounterDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.Name.ShouldBe(newEntity.Name);

            // Assert - Database
            var storedEntity = dbContext.Encounters.FirstOrDefault(i => i.Name == newEntity.Name);
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
            var updatedEntity = new EncounterDto
            {
                Id = -1,
                UserId = 1,
                Name = "UpdatedEncounter4",
                Description = "UpdatedDescription4",
                Latitude = 55.55,
                Longitude = 55.55,
                Xp = 5,
                Status = "ACTIVE",
                Type = "MISC",
                ApprovalStatus = "SYSTEM_APPROVED"
            };

            // Act
            var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as EncounterDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(-1);
            result.Name.ShouldBe(updatedEntity.Name);
            result.Description.ShouldBe(updatedEntity.Description);

            // Assert - Database
            var storedEntity = dbContext.Encounters.FirstOrDefault(i => i.Name == "UpdatedEncounter4");
            storedEntity.ShouldNotBeNull();
            storedEntity.Description.ShouldBe(updatedEntity.Description);
            var oldEntity = dbContext.Encounters.FirstOrDefault(i => i.Name == "Encounter4");
            oldEntity.ShouldBeNull();
        }


        [Fact]
        public void Update_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new EncounterDto
            {
                Id = -1000,
                Name = "UpdatedEquipment"
            };

            // Act
            var result = (ObjectResult)controller.Update(updatedEntity).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }


        [Fact]
        public void Deletes()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();

            // Act
            var result = (OkResult)controller.Delete(-3);

            // Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            // Assert - Database
            var storedCourse = dbContext.Encounters.FirstOrDefault(i => i.Id == -3);
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

        private static EncounterController CreateController(IServiceScope scope)
        {
            return new EncounterController(scope.ServiceProvider.GetRequiredService<IEncounterService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
   
    }
}
