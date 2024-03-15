using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories;

public class ClubJoinRequestDatabaseRepository : CrudDatabaseRepository<ClubJoinRequest, StakeholdersContext>,
    IClubJoinRequestRepository
{
    private readonly DbSet<ClubJoinRequest> _dbSet;
    protected readonly StakeholdersContext DbContext;

    public ClubJoinRequestDatabaseRepository(StakeholdersContext dbContext) : base(dbContext)
    {
        DbContext = dbContext;
        _dbSet = DbContext.Set<ClubJoinRequest>();
    }

    public PagedResult<ClubJoinRequest> GetAllByUser(long userId)
    {
        var requests = _dbSet.AsNoTracking().Where(jr => jr.UserId == userId).ToList();
        return new PagedResult<ClubJoinRequest>(requests, requests.Count);
    }

    public PagedResult<ClubJoinRequest> GetAllByClub(long clubId)
    {
        var requests = _dbSet.AsNoTracking().Where(jr => jr.ClubId == clubId).ToList();
        return new PagedResult<ClubJoinRequest>(requests, requests.Count);
    }

    public bool Exists(long clubId, long userId)
    {
        return DbContext.ClubJoinRequests.Any(request => request.ClubId == clubId && request.UserId == userId &&
                                                         request.Status == JoinRequestStatus.Pending);
    }
}