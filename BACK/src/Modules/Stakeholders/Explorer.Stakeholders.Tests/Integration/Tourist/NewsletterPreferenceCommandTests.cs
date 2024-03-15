using Explorer.API.Controllers.Tourist.TourExecution;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.TourExecution;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Explorer.Stakeholders.Infrastructure.Database;
using System.Runtime.CompilerServices;
using Explorer.API.Controllers.Tourist;
using Explorer.Stakeholders.API.Public.Tourist;

namespace Explorer.Stakeholders.Tests.Integration.Tourist
{
    public class NewsletterPreferenceCommandTests : BaseStakeholdersIntegrationTest
    {
        public NewsletterPreferenceCommandTests(StakeholdersTestFactory factory) : base(factory) { }
        
        [Fact]
        public void Creates()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var newEntity = new NewsletterPreferenceDto
            {
                UserID = -13,
                Frequency = 0,
                LastSent = DateTime.Now.ToUniversalTime()
            };

            //Act
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as NewsletterPreferenceDto;

            //Assert
            result.ShouldNotBeNull();
            result.UserID.ShouldBe(result.UserID);
            result.Frequency.ShouldBe(result.Frequency);
            result.LastSent.ShouldBe(result.LastSent);
        }

        /*[Fact]
        public void Create_fails_invalid_data()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new NewsletterPreferenceDto
            {
                UserID = -1,
                Frequency = 2,
                LastSent = DateTime.Now.ToUniversalTime()
            };

            //Act
            var result = (ObjectResult)controller.Create(updatedEntity).Result;

            //Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(400);
        }*/

        [Fact]
        public void Updates()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var updatedEntity = new NewsletterPreferenceDto
            {
                UserID = -21,
                Frequency = 1,
                LastSent = DateTime.Now.ToUniversalTime()
            };

            //Act
            var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as NewsletterPreferenceDto;

            //Assert - Response
            result.ShouldNotBeNull();
            result.UserID.ShouldBe(-21);
            result.Frequency.ShouldBe((uint)1);
            result.LastSent.ShouldBe(updatedEntity.LastSent);

            //Assert - Database
            var storedEntity = dbContext.NewsletterPreferences.FirstOrDefault(i => i.UserID == -21 && i.Frequency == 1);
            storedEntity.ShouldNotBeNull();
            storedEntity.Frequency.ShouldBe((uint)1);
            var oldEntity = dbContext.NewsletterPreferences.FirstOrDefault(i => i.UserID == -21 && i.Frequency == 0);
            oldEntity.ShouldBeNull();
        }

        [Fact]
        public void Update_fails_invalid_id()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new NewsletterPreferenceDto
            {
                UserID = -21,
                Frequency = 0,
                LastSent = DateTime.Now.ToUniversalTime()
            };

            //Act 
            var result = (ObjectResult)controller.Update(updatedEntity).Result;

            //Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }

        [Fact]
        public void Deletes()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            //Act
            var result = (OkResult)controller.Delete(-1);

            //Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            //Assert - Database
            var storedCourse = dbContext.NewsletterPreferences.FirstOrDefault(i => i.Id == -22);
            storedCourse.ShouldBeNull();
        }

        [Fact]
        public void Delete_fails_invalid_id()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            //Act
            var result = (ObjectResult)controller.Delete(-1000);

            //Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }

        private static NewsletterPreferenceController CreateController(IServiceScope scope)
        {
            return new NewsletterPreferenceController(scope.ServiceProvider.GetRequiredService<INewsletterPreferenceService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
