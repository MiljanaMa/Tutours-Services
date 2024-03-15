using Explorer.API.Controllers.Tourist.Shopping;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.MarketPlace;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Infrastructure.Database;
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
    public class BundleCommandTests : BaseToursIntegrationTest
    {
        public BundleCommandTests(ToursTestFactory factory) : base(factory) { }

        private static BundleController CreateController(IServiceScope scope)
        {
            return new BundleController(scope.ServiceProvider.GetRequiredService<IBundleService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }

        [Fact]
        public void Create()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newBundle = new BundleDto { Id = -8, Name = "Test Bundle", Status = "DRAFT" };

            // Act
            var result = ((ObjectResult)controller.Create(newBundle).Result)?.Value as BundleDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.Name.ShouldBe(newBundle.Name);
            result.Status.ShouldBe(newBundle.Status);

            // Assert - Database
            var storedEntity = dbContext.Bundle.FirstOrDefault(i => i.Id == result.Id);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.Id);
            storedEntity.Name.ShouldBe(newBundle.Name);
            storedEntity.Status.ShouldBe(newBundle.Status);
        }

        [Fact]
        public void Create_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            // Invalid data: Missing Name
            var invalidBundle = new BundleDto { Id = -8, Status = "DRAFT" };

            // Act & Assert
            Should.Throw<ArgumentException>(() => controller.Create(invalidBundle).Result);
        }

        [Fact]
        public void Deletes()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var entity = new Bundle(-1, "Bundle 1", "DRAFT");

            // Act
            var result = (OkResult)controller.Delete((int)entity.Id);

            // Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            // Assert - Database
            var storedCourse = dbContext.Bundle.FirstOrDefault(i => i.Id == -1);
            storedCourse.ShouldBeNull();
        }

    }
}

