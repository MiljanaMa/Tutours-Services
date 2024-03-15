using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories;

public class TourIssueDatabaseRepository : ITourIssueRepository
{
    private readonly DbSet<TourIssue> _dbSet;
    protected readonly StakeholdersContext _dbContext;

    public TourIssueDatabaseRepository(StakeholdersContext DbContext)
    {
        _dbContext = DbContext;
        _dbSet = _dbContext.Set<TourIssue>();
    }

    public PagedResult<TourIssue> GetPaged(int page, int pageSize)
    {
        var task = _dbSet.GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public TourIssue Get(long id)
    {
        var entity = _dbSet.Find(id);
        if (entity == null) throw new KeyNotFoundException("Not found: " + id);
        return entity;
    }

    public TourIssue Create(TourIssue entity)
    {
        _dbSet.Add(entity);
        _dbContext.SaveChanges();
        return entity;
    }

    public TourIssue Update(TourIssue entity)
    {
        try
        {
            _dbContext.Update(entity);
            _dbContext.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            throw new KeyNotFoundException(e.Message);
        }
        return entity;
    }

    public void Delete(long id)
    {
        var entity = Get(id);
        _dbSet.Remove(entity);
        _dbContext.SaveChanges();
    }

    public PagedResult<TourIssue> GetByUserPaged(int page, int pageSize, int userId)
    {
        var task = _dbSet.Where(t => t.UserId == userId).Include(t => t.Comments).GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public PagedResult<TourIssue> GetByTourId(int page, int pageSize, int tourId)
    {
        var task = _dbSet.Where(t => t.TourId == tourId).Include(t => t.Comments).GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public PagedResult<TourIssue> GetByTourIssueId(int page, int pageSize, int tourIssueId)
    {
        var task = _dbSet.Where(t => t.Id == tourIssueId).Include(t => t.Comments).GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }
}