using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

public interface IClubRepository: ICrudRepository<Club>
{
    public Club GetUntracked(long id);
    public PagedResult<Club> GetAll(int page, int pageSize);
    public Club GetWithMembers(long id);
}