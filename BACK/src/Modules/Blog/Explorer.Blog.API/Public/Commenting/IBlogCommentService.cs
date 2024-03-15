using Explorer.Blog.API.Dtos;
using Explorer.BuildingBlocks.Core.UseCases;
using FluentResults;

namespace Explorer.Blog.API.Public.Commenting;

public interface IBlogCommentService
{
    Result<PagedResult<BlogCommentDto>> GetPaged(int page, int pageSize, long blogId);
    Result<BlogCommentDto> Get(int id, long userId);
    Result<BlogCommentDto> Create(BlogCommentDto blogCommentDto);
    Result<BlogCommentDto> Update(BlogCommentDto blogCommentDto,long userId);
    Result Delete(int id);
}