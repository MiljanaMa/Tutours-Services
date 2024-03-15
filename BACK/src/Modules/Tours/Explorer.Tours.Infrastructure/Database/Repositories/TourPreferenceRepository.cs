using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Tours.Infrastructure.Database.Repositories;

public class TourPreferenceRepository : CrudDatabaseRepository<TourPreference, ToursContext>, ITourPreferenceRepository
{
    private readonly DbSet<TourPreference> _dbSet;

    public TourPreferenceRepository(ToursContext dbContext) : base(dbContext)
    {
        _dbSet = DbContext.Set<TourPreference>();
    }

    public TourPreference GetByUser(long userId)
    {
        var tourPreference = _dbSet.AsNoTracking().FirstOrDefault(tp => tp.UserId == userId);
        if (tourPreference == null) throw new KeyNotFoundException("Not found: " + userId);
        return tourPreference;
    }
}