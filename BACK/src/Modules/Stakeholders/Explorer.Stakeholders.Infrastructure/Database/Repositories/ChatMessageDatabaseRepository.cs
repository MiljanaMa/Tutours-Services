using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories;

public class ChatMessageDatabaseRepository : CrudDatabaseRepository<ChatMessage, StakeholdersContext>,
    IChatMessageRepository
{
    private readonly DbSet<ChatMessage> _dbSet;

    public ChatMessageDatabaseRepository(StakeholdersContext dbContext) : base(dbContext)
    {
        _dbSet = dbContext.Set<ChatMessage>();
    }

    public PagedResult<ChatMessage> GetConversation(long firstParticipantId, long secondParticipantId)
    {
        var chatMessages = _dbSet
            .AsNoTracking()
            .Where(cm => (cm.SenderId == firstParticipantId && cm.ReceiverId == secondParticipantId) ||
                         (cm.SenderId == secondParticipantId && cm.ReceiverId == firstParticipantId))
            .OrderByDescending(cm => cm.CreationDateTime)
            .Include("Sender")
            .Include("Receiver")
            .ToList();

        return new PagedResult<ChatMessage>(chatMessages, chatMessages.Count);
    }

    public IEnumerable<ChatMessage> GetPreviewMessages(long userId)
    {
        var chatMessages = _dbSet
            .AsNoTracking()
            .Where(c => c.SenderId == userId || c.ReceiverId == userId)
            .OrderBy(c => c.SenderId).ThenBy(c => c.ReceiverId).ThenByDescending(c => c.CreationDateTime)
            .Include("Sender")
            .Include("Receiver").ToList();

        return RemovePreviewDuplicates(chatMessages);
    }

    public IEnumerable<ChatMessage> GetChatUnreadMessages(long senderId, long receiverId)
    {
        var chatMessages = _dbSet
            .AsNoTracking()
            .Where(cm => cm.SenderId == senderId && cm.ReceiverId == receiverId && cm.IsRead == false)
            .ToList();

        return chatMessages;
    }

    private IEnumerable<ChatMessage> RemovePreviewDuplicates(IEnumerable<ChatMessage> messages)
    {
        var reducedMessages = new List<ChatMessage>();
        var distinctMessages = messages.DistinctBy(c => new { c.SenderId, c.ReceiverId }).ToList();
        foreach (var chatm in distinctMessages)
        {
            if (IsReverseMessageOrIdentical(reducedMessages, chatm))
                continue;

            var newerMessage = GetNewerMessage(distinctMessages, chatm);
            reducedMessages.Add(newerMessage);
        }

        return reducedMessages.OrderByDescending(m => m.CreationDateTime);
    }

    private bool IsReverseMessageOrIdentical(List<ChatMessage> reducedMessages, ChatMessage chatm)
    {
        if (reducedMessages.Count > 0)
            if (reducedMessages.FirstOrDefault(m =>
                    (m.SenderId == chatm.ReceiverId && m.ReceiverId == chatm.SenderId) || m.Id == chatm.Id) != null)
                return true;
        return false;
    }

    private ChatMessage GetNewerMessage(List<ChatMessage> distinctMessages, ChatMessage chatm)
    {
        var potentialReverseMessage = distinctMessages
            .Where(m => m.SenderId == chatm.ReceiverId && m.ReceiverId == chatm.SenderId)
            .OrderByDescending(m => m.CreationDateTime)
            .FirstOrDefault();
        if (potentialReverseMessage != null)
            if (DateTime.Compare(potentialReverseMessage.CreationDateTime, chatm.CreationDateTime) > 0)
                return potentialReverseMessage;
        return chatm;
    }
}