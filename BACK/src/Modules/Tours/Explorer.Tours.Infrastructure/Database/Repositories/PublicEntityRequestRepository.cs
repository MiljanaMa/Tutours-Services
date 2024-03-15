using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.Core.Domain.Enum;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Tours.Infrastructure.Database.Repositories;

public class PublicEntityRequestRepository : CrudDatabaseRepository<PublicEntityRequest, ToursContext>,
    IPublicEntityRequestRepository
{
    private readonly DbSet<PublicEntityRequest> _dbSet;

    public PublicEntityRequestRepository(ToursContext dbContext) : base(dbContext)
    {
        _dbSet = dbContext.Set<PublicEntityRequest>();
    }

    public PublicEntityRequest GetByEntityId(int entityId, EntityType entityType)
    {
        if (entityType == EntityType.KEYPOINT)
        {
            var publicEntityRequest = _dbSet.AsNoTracking()
                .FirstOrDefault(r =>
                    r.EntityId == entityId && r.EntityType == (Core.Domain.Enum.EntityType)EntityType.KEYPOINT);
            return publicEntityRequest;
        }

        if (entityType == EntityType.OBJECT)
        {
            var publicEntityRequest = _dbSet.AsNoTracking()
                .FirstOrDefault(r =>
                    r.EntityId == entityId && r.EntityType == (Core.Domain.Enum.EntityType)EntityType.OBJECT);
            return publicEntityRequest;
        }

        return null;
    }
}