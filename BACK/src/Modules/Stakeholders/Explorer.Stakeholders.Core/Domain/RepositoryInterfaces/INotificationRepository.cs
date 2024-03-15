using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

public interface INotificationRepository
{
    public PagedResult<Notification> GetByUser(int page, int pageSize, int userId);
    public void Generate(Notification notification);
    public void MarkAsRead(long id);
    public void Delete(long id);
}