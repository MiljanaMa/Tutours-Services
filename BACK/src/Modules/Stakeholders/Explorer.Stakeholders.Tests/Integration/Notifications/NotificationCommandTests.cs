using Explorer.API.Controllers;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.Notifications;

[Collection("Sequential")]
public class NotificationCommandTests : BaseStakeholdersIntegrationTest
{
    public NotificationCommandTests(StakeholdersTestFactory factory) : base(factory)
    {
    }


    [Fact]
    public void Updates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        // Act
        var result = (OkResult)controller.MarkAsRead(-1);

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);


        // Assert - Database
        var storedEntity = dbContext.Notifications.FirstOrDefault(n => n.Id == -1);
        storedEntity.ShouldNotBeNull();
        storedEntity.IsRead.ShouldBe(true);
    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = (ObjectResult)controller.MarkAsRead(-1000);

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
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        // Act
        var result = (OkResult)controller.Delete(-3);

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedCourse = dbContext.Notifications.FirstOrDefault(n => n.Id == -3);
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

    private static NotificationController CreateController(IServiceScope scope)
    {
        return new NotificationController(scope.ServiceProvider.GetRequiredService<INotificationService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}