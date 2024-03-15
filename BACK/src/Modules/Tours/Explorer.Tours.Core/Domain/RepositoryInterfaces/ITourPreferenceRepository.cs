using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces;

public interface ITourPreferenceRepository : ICrudRepository<TourPreference>
{
    TourPreference GetByUser(long userId);
}