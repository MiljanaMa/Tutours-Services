using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;

namespace Explorer.Stakeholders.API.Public.Identity;

public interface IChatMessageService
{
    Result<PagedResult<ChatMessageDto>> GetConversation(long firstParticipantId, long secondParticipantId);
    Result<ChatMessageDto> Create(long senderId, MessageDto message);
    Result<List<ChatMessageDto>> GetPreviewMessages(long userId);
}