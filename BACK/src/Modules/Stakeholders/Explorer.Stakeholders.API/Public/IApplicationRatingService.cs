using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;

namespace Explorer.Stakeholders.API.Public;

public interface IApplicationRatingService
{
    Result<PagedResult<ApplicationRatingDto>> GetPaged(int page, int pageSize);
    Result<ApplicationRatingDto> GetByUser(int userId);
    Result<ApplicationRatingDto> Create(ApplicationRatingDto applicationRating);
    Result<ApplicationRatingDto> Update(ApplicationRatingDto applicationRating);
    Result Delete(int userId);
}