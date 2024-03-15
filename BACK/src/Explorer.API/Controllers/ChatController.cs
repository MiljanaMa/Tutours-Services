using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Identity;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers;

[Authorize(Policy = "personPolicy")]
[Route("api/chat")]
public class ChatController : BaseApiController
{
    private readonly IChatMessageService _chatService;

    public ChatController(IChatMessageService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet("{participantId:long}")]
    public ActionResult<PagedResult<ChatMessageDto>> GetConversation(long participantId)
    {
        var result = _chatService.GetConversation(User.PersonId(), participantId);
        return CreateResponse(result);
    }

    [HttpPost]
    public ActionResult<ChatMessageDto> Create([FromBody] MessageDto message)
    {
        var result = _chatService.Create(User.PersonId(), message);
        return CreateResponse(result);
    }

    [HttpGet("preview")]
    public ActionResult<List<ChatMessageDto>> GetPreviewMessages()
    {
        var result = _chatService.GetPreviewMessages(User.PersonId());
        return CreateResponse(result);
    }
}