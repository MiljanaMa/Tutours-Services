using Explorer.API.Controllers.Tourist;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration;

public class TouristEqipmentCommandTests : BaseToursIntegrationTest
{
    public TouristEqipmentCommandTests(ToursTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Creates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var newEntity = new TouristEquipmentDto(-3, 2, -3);


        // Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TouristEquipmentDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.TouristId.ShouldNotBe(0);
        result.EquipmentId.ShouldNotBe(0);

        // Assert - Database
        var storedEntity = dbContext.TouristEquipment.FirstOrDefault(i => i.Id == newEntity.Id);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    [Fact]
    public void Create_fails_invalid_data()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new TouristEquipmentDto(-4, 5, -9);

        // Act
        var result = (ObjectResult)controller.Create(updatedEntity).Result;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(400);
    }


    [Fact]
    public void Deletes()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var entity = new TouristEquipmentDto(-1, -23, -1);

        // Act
        var result = (OkResult)controller.Delete(entity);

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedCourse = dbContext.TouristEquipment.FirstOrDefault(i => i.Id == -23);
        storedCourse.ShouldBeNull();
    }

    [Fact]
    public void Delete_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var entity = new TouristEquipmentDto(-10, 2, -30);

        // Act
        var result = (ObjectResult)controller.Delete(entity);

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    private static TouristEquipmentController CreateController(IServiceScope scope)
    {
        return new TouristEquipmentController(scope.ServiceProvider.GetRequiredService<ITouristEquipmentService>(),
            scope.ServiceProvider.GetRequiredService<IEquipmentService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}