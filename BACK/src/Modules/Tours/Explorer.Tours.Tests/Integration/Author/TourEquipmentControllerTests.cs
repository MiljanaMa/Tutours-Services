using Explorer.API.Controllers.Author;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourAuthoring;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.Author;

[Collection("Sequential")]
public class TourEquipmentControllerTests : BaseToursIntegrationTest
{
    public TourEquipmentControllerTests(ToursTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void AddsEquipmentToTour()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        var newEntity = new TourEquipmentDto
        {
            TourId = -2,
            EquipmentId = -1
        };

        // Act
        var result = ((ObjectResult)controller.AddEquipmentToTour(newEntity).Result)?.Value as TourEquipmentDto;

        // Assert - Database
        var storedEntity = dbContext.TourEquipments.FirstOrDefault(i =>
            i.TourId == newEntity.TourId && i.EquipmentId == newEntity.EquipmentId);
        storedEntity.ShouldNotBeNull();
    }


    [Fact]
    public void GetsEquipmentForTour()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var tourId = -1;

        // Act
        var result = ((ObjectResult)controller.GetEquipmentForTour(tourId).Result)?.Value as List<EquipmentDto>;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public void RemovesEquipmentFromTour()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var entityToRemove = new TourEquipmentDto
        {
            TourId = -1,
            EquipmentId = -1
        };

        // Act
        var result = (OkResult)controller.RemoveEquipmentFromTour(entityToRemove);

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var deletedEntity = dbContext.TourEquipments.FirstOrDefault(i =>
            i.TourId == entityToRemove.TourId && i.EquipmentId == entityToRemove.EquipmentId);
        deletedEntity.ShouldBeNull();
    }


    private static TourEquipmentController CreateController(IServiceScope scope)
    {
        return new TourEquipmentController(scope.ServiceProvider.GetRequiredService<ITourEquipmentService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}