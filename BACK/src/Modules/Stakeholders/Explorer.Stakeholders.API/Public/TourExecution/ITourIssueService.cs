using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;

namespace Explorer.Stakeholders.API.Public.TourExecution;

public interface ITourIssueService
{
    public Result<PagedResult<TourIssueDto>> GetPaged(int page, int pageSize);
    public Result<TourIssueDto> Get(int id);
    public Result<TourIssueDto> Create(TourIssueDto entity);
    public Result<TourIssueDto> Update(TourIssueDto entity);
    public Result Delete(int id);
    public Result<PagedResult<TourIssueDto>> GetByUserPaged(int page, int pageSize, int id);
    public Result<PagedResult<TourIssueDto>> GetByTourId(int page, int pageSize, int tourId);
    public Result<PagedResult<TourIssueDto>> GetByTourIssueId(int page, int pageSize, int tourIssueId);
    public Result<TourIssueDto> SetResolvedDateTime(TourIssueDto entity);
}