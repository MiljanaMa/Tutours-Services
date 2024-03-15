using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

public interface IChatMessageRepository : ICrudRepository<ChatMessage>
{
    PagedResult<ChatMessage> GetConversation(long firstParticipantId, long secondParticipantId);
    IEnumerable<ChatMessage> GetPreviewMessages(long userId);
    IEnumerable<ChatMessage> GetChatUnreadMessages(long senderId, long receiverId);
}