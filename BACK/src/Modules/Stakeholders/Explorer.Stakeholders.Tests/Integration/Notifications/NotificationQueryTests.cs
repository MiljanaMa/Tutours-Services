using Explorer.API.Controllers;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.Notifications;

[Collection("Sequential")]
public class NotificationQueryTests : BaseStakeholdersIntegrationTest
{
    public NotificationQueryTests(StakeholdersTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Retrieves_all()
    {
        // Arrange
        var userId = 1;
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        controller.ControllerContext = BuildContext(userId.ToString());

        // Act
        var result = ((ObjectResult)controller.GetByUser(0, 0).Result)?.Value as PagedResult<NotificationDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(3);
        result.TotalCount.ShouldBe(3);
    }

    private static NotificationController CreateController(IServiceScope scope)
    {
        return new NotificationController(scope.ServiceProvider.GetRequiredService<INotificationService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}