using AutoMapper;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public.Blog;
using Explorer.Blog.Core.Domain;
using Explorer.BuildingBlocks.Core.UseCases;
using FluentResults;

namespace Explorer.Blog.Core.UseCases.Blog;

public class BlogStatusService : CrudService<BlogStatusDto, BlogStatus>, IBlogStatusService
{
    public BlogStatusService(ICrudRepository<BlogStatus> crudRepository, IMapper mapper) : base(crudRepository, mapper)
    {
    }

    public Result<BlogStatusDto> Generate(long blogId, string name)
    {
        var blogStatus = new BlogStatus(blogId, name);
        return Create(MapToDto(blogStatus));
    }
}