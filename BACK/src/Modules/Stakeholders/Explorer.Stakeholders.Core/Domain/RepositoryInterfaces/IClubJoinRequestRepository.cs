using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

public interface IClubJoinRequestRepository : ICrudRepository<ClubJoinRequest>
{
    PagedResult<ClubJoinRequest> GetAllByUser(long userId);
    PagedResult<ClubJoinRequest> GetAllByClub(long clubId);
    bool Exists(long clubId, long userId);
}