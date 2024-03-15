using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain.Enums;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IAchievementRepository : ICrudRepository<Achievement>
    {
        Achievement GetByType(AchievementType type);
        Achievement GetNoTracking(int id);
    }
}
