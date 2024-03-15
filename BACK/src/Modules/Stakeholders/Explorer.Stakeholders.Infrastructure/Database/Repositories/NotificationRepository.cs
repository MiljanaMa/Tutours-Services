using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly StakeholdersContext _dbContext;

    public NotificationRepository(StakeholdersContext dbContext)
    {
        _dbContext = dbContext;
    }

    public PagedResult<Notification> GetByUser(int page, int pageSize, int userId)
    {
        var task = _dbContext.Notifications.Where(n => n.UserId == userId).GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public void Generate(Notification notification)
    {
        _dbContext.Notifications.Add(notification);
        _dbContext.SaveChanges();
    }

    public void MarkAsRead(long id)
    {
        try
        {
            var notification = Get(id);
            notification.MarkAsRead();
            _dbContext.Update(notification);
            _dbContext.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            throw new KeyNotFoundException(e.Message);
        }
    }

    public void Delete(long id)
    {
        var notification = Get(id);
        _dbContext.Notifications.Remove(notification);
        _dbContext.SaveChanges();
    }

    public Notification Get(long id)
    {
        var notification = _dbContext.Notifications.Find(id);
        if (notification == null) throw new KeyNotFoundException("Not found: " + id);
        return notification;
    }
}