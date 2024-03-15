using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Tours.Infrastructure.Database.Repositories;

public class KeypointRepository : CrudDatabaseRepository<Keypoint, ToursContext>, IKeypointRepository
{
    private readonly DbSet<Keypoint> _dbSet;

    public KeypointRepository(ToursContext dbContext) : base(dbContext)
    {
        _dbSet = dbContext.Set<Keypoint>();
    }

    public PagedResult<Keypoint> GetByTour(int page, int pageSize, int tourId)
    {
        var task = _dbSet.Where(k => k.TourId == tourId).GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public IEnumerable<Keypoint> GetByTourAndPosition(long tourId, int? currentPosition)
    {
        return _dbSet.Where(k => k.TourId == tourId && k.Position == currentPosition);
    }

    public IEnumerable<int?> GetNextPositions(long tourId, int? currentPosition)
    {
        return _dbSet.Where(k => k.TourId == tourId && k.Position > currentPosition).Select(k => k.Position)
            .OrderBy(p => p);
    }
}