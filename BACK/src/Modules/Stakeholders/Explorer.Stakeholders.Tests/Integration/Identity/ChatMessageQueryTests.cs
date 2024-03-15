using Explorer.API.Controllers;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.Identity;

[Collection("Sequential")]
public class ChatMessageQueryTests : BaseStakeholdersIntegrationTest
{
    public ChatMessageQueryTests(StakeholdersTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Retrieves_all()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetConversation(-22).Result)?.Value as PagedResult<ChatMessageDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(3);
        result.TotalCount.ShouldBe(3);
    }

    [Fact]
    public void Retrieves_previews()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetPreviewMessages().Result)?.Value as List<ChatMessageDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
    }

    private static ChatController CreateController(IServiceScope scope)
    {
        return new ChatController(scope.ServiceProvider.GetRequiredService<IChatMessageService>())
        {
            ControllerContext = BuildContext("-21")
        };
    }
}