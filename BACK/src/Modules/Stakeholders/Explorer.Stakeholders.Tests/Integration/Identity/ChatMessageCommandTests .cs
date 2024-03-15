using Explorer.API.Controllers;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Identity;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.Identity;

public class ChatMessageCommandTests : BaseStakeholdersIntegrationTest
{
    public ChatMessageCommandTests(StakeholdersTestFactory factory) : base(factory)
    {
    }

    [Theory]
    [InlineData(-22, -21, "Pozdrav", 200, 2)]
    [InlineData(-21, -25, "Pozdrav", 404, 0)]
    public void Create(int senderId, int receiverId, string content, int expectedResponseCode,
        int expectedchatMessagesCount)
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, senderId);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var message = new MessageDto
        {
            ReceiverId = receiverId,
            Content = content
        };

        var result = (ObjectResult)controller.Create(message).Result;

        result.StatusCode.ShouldBe(expectedResponseCode);

        var recieverMessages = dbContext.ChatMessages.Where(i => i.ReceiverId == receiverId).ToList();
        recieverMessages.Count.ShouldBe(expectedchatMessagesCount);
    }

    private static ChatController CreateController(IServiceScope scope, int userId)
    {
        return new ChatController(scope.ServiceProvider.GetRequiredService<IChatMessageService>())
        {
            ControllerContext = BuildContext(userId.ToString())
        };
    }
}