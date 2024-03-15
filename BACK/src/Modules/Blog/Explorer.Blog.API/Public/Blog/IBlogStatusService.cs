using Explorer.Blog.API.Dtos;
using Explorer.BuildingBlocks.Core.UseCases;
using FluentResults;

namespace Explorer.Blog.API.Public.Blog;

public interface IBlogStatusService
{
    public Result<BlogStatusDto> Create(BlogStatusDto blogStatus);
    public Result<BlogStatusDto> Get(int id);
    public Result<PagedResult<BlogStatusDto>> GetPaged(int page, int pageSize);
    public Result<BlogStatusDto> Update(BlogStatusDto blogStatus);
    public Result Delete(int id);
    public Result<BlogStatusDto> Generate(long blogId, string name);
}