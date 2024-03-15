using Explorer.API.Controllers.Author;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Dtos.Enums;
using Explorer.Tours.API.Public.TourAuthoring;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

//using Explorer.Tours.API.Public.Object;

namespace Explorer.Tours.Tests.Integration.TourAuthoring;

[Collection("Sequential")]
public class ObjectCommandTests : BaseToursIntegrationTest
{
    public ObjectCommandTests(ToursTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Creates()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var newEntity = new ObjectDto
        {
            Name = "Test",
            Description = "Test",
            Image = "Test",
            Category = "WC",
            Status = ObjectStatus.PRIVATE
        };

        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as ObjectDto;

        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Name.ShouldBe(newEntity.Name);

        var storedEntity = dbContext.Objects.FirstOrDefault(i => i.Name == newEntity.Name);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    [Fact]
    public void Create_fails_invalid_data()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new ObjectDto
        {
            Name = string.Empty
        };

        // Act
        var result = (ObjectResult)controller.Create(updatedEntity).Result;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(400);
    }

    [Fact]
    public void Updates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var updatedEntity = new ObjectDto
        {
            Id = -1,
            Name = "Test",
            Description = "Test",
            Image = "Test",
            Category = "WC",
            Status = ObjectStatus.PRIVATE
        };

        var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as ObjectDto;

        result.ShouldNotBeNull();
        result.Id.ShouldBe(-1);
        result.Name.ShouldBe(updatedEntity.Name);
        result.Description.ShouldBe(updatedEntity.Description);
        result.Image.ShouldBe(updatedEntity.Image);
        result.Category.ShouldBe(updatedEntity.Category);
        result.Status.ShouldBe(updatedEntity.Status);

        var storedEntity = dbContext.Objects.FirstOrDefault(i => i.Name == updatedEntity.Name);
        storedEntity.ShouldNotBeNull();
        storedEntity.Description.ShouldBe(updatedEntity.Description);
        var oldEntity = dbContext.Objects.FirstOrDefault(i => i.Name == "Test1");
        oldEntity.ShouldBeNull();
    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new ObjectDto
        {
            Id = -15,
            Name = "Test",
            Description = "Test",
            Image = "Test",
            Category = "WC",
            Status = ObjectStatus.PRIVATE
        };

        var result = (ObjectResult)controller.Update(updatedEntity).Result;

        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    [Fact]
    public void Deletes()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        var result = (OkResult)controller.Delete(-3);

        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        var storedCourse = dbContext.Objects.FirstOrDefault(i => i.Id == -3);
        storedCourse.ShouldBeNull();
    }

    [Fact]
    public void Delete_fails_invalid_id()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var result = (ObjectResult)controller.Delete(-1000);

        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }


    private static ObjectController CreateController(IServiceScope scope)
    {
        return new ObjectController(scope.ServiceProvider.GetRequiredService<IObjectService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}