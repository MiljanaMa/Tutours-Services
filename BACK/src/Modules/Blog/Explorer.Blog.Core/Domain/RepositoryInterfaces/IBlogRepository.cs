using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Blog.Core.Domain.RepositoryInterfaces;

public interface IBlogRepository : ICrudRepository<Blog>
{
    public PagedResult<Blog> GetWithStatuses(int page, int pageSize);
    Blog GetBlog(long id);
}