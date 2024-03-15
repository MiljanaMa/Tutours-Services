using Explorer.Blog.API.Dtos;
using Explorer.BuildingBlocks.Core.UseCases;
using FluentResults;

namespace Explorer.Blog.API.Public.Blog;

public interface IBlogService
{
    public Result<BlogDto> Create(BlogDto blog);
    public Result<BlogDto> Get(int id);
    public Result<PagedResult<BlogDto>> GetPaged(int page, int pageSize);
    public Result<BlogDto> Update(BlogDto blog);
    public Result Delete(int id);
    public PagedResult<BlogDto> GetWithStatuses(int page, int pageSize);
    public Result<BlogDto> AddRating(BlogRatingDto blogRatingDto, long userId);
    public void UpdateStatuses(BlogDto blogDto, string status);
}
