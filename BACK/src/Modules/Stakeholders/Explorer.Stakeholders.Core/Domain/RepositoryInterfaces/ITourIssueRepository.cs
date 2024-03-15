using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

public interface ITourIssueRepository : ICrudRepository<TourIssue>
{
    public PagedResult<TourIssue> GetByUserPaged(int page, int pageSize, int userId);
    public PagedResult<TourIssue> GetByTourId(int page, int pageSize, int tourId);
    public PagedResult<TourIssue> GetByTourIssueId(int page, int pageSize, int tourIssueId);
}