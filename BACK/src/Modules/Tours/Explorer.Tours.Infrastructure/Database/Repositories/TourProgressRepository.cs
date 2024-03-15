using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.Enum;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace Explorer.Tours.Infrastructure.Database.Repositories;

public class TourProgressRepository : CrudDatabaseRepository<TourProgress, ToursContext>, ITourProgressRepository
{
    private readonly DbSet<TourProgress> _dbSet;

    public TourProgressRepository(ToursContext dbContext) : base(dbContext)
    {
        _dbSet = dbContext.Set<TourProgress>();
    }

    public TourProgress GetActiveByUser(long userId)
    {
        var tourProgress = _dbSet
            .Include(tp => tp.TouristPosition)
            .Include(tp => tp.Tour)
            .ThenInclude(t => t.Keypoints)
            .FirstOrDefault(tp => tp.TouristPosition.UserId == userId && tp.Status == TourProgressStatus.IN_PROGRESS);
        if (tourProgress == null) throw new KeyNotFoundException("Not found: " + userId);
        return tourProgress;
    }

    public TourProgress GetByUser(long userId)
    {
        var tourProgress = _dbSet
            .Include(tp => tp.TouristPosition)
            .Include(tp => tp.Tour)
            .ThenInclude(t => t.Keypoints)
            .FirstOrDefault(tp => tp.TouristPosition.UserId == userId);
        if (tourProgress == null) throw new KeyNotFoundException("Not found: " + userId);
        return tourProgress;
    }

    public int GetCompletedCountByUserAndMonth(long userId, int month, int year)
    {
        return _dbSet.Where(tp => tp.TouristPosition.UserId == userId && tp.Status == TourProgressStatus.COMPLETED
           && tp.LastActivity.Month == month && tp.LastActivity.Year == year).Count();
    }
}