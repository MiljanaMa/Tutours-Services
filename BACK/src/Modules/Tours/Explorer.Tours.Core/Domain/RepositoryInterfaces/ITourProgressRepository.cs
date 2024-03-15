using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces;

public interface ITourProgressRepository : ICrudRepository<TourProgress>
{
    TourProgress GetActiveByUser(long userId);
    public TourProgress GetByUser(long userId);
    public int GetCompletedCountByUserAndMonth(long userId, int month, int year);
}