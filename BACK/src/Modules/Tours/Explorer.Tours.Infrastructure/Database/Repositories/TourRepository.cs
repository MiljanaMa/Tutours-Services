using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.Enum;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Tours.Infrastructure.Database.Repositories;

public class TourRepository : CrudDatabaseRepository<Tour, ToursContext>, ITourRepository
{
    private readonly DbSet<Tour> _dbSet;

    public TourRepository(ToursContext dbContext) : base(dbContext)
    {
        _dbSet = dbContext.Set<Tour>();
    }

    public PagedResult<Tour> GetByAuthorPaged(int page, int pageSize, int authorId)
    {
        var task = _dbSet.Where(t => t.UserId == authorId).GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public PagedResult<Tour> GetPublishedPaged(int page, int pageSize)
    {
        var task = _dbSet.Where(t => t.Status == TourStatus.PUBLISHED).Include(t => t.Keypoints)
            .GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public PagedResult<Tour> GetArchivedAndPublishedPaged(int page, int pageSize)
    {
        var task = _dbSet.Where(t => t.Status == TourStatus.PUBLISHED || t.Status == TourStatus.ARCHIVED)
            .Include(t => t.Keypoints)
            .GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }
    
    public PagedResult<Tour> GetCustomByUserPaged(int userId, int page, int pageSize)
    {
        var task = _dbSet.Where(t => (t.Status == TourStatus.CUSTOM || t.Status == TourStatus.DRAFT) && t.UserId == userId)
            .Include(t => t.Keypoints)
            .GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public PagedResult<Tour> GetCampaignByUserPaged(int userId, int page, int pageSize)
    {
        var task = _dbSet.Where(t => t.Status == TourStatus.CAMPAIGN && t.UserId == userId)
            .Include(t => t.Keypoints)
            .GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public PagedResult<Tour> GetPublishedByAuthor(int authorId, int page, int pageSize)
    {
        var task = _dbSet.Where(t => t.Status == TourStatus.PUBLISHED && t.UserId == authorId)
            .GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }
}